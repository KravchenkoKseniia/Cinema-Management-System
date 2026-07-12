using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Cinema_System.DTOs;
using Infrastructure.Data;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MySql;

namespace CinemaSystem.Test.Integration;
public sealed class TicketApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MySqlContainer _mySql = new MySqlBuilder()
        .WithImage("mysql:8.0")
        .Build();

    public int SeededUserId { get; private set; }
    public int SeededSessionId { get; private set; }
    public int SeededHallId { get; private set; }
    public int SeededPaymentId { get; private set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:DefaultConnection", _mySql.GetConnectionString());
    }

    public async ValueTask InitializeAsync()
    {
        await _mySql.StartAsync();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseMySql(_mySql.GetConnectionString(), ServerVersion.AutoDetect(_mySql.GetConnectionString()))
            .Options;

        await using var context = new ApplicationDbContext(options);
        await context.Database.MigrateAsync();

        var hall = new Hall { Name = "Contract Hall", Capacity = 100 };
        var movie = new Movie
        {
            Title = "Contract Movie",
            Description = "Seeded for the booking contract test.",
            TrailerURL = "https://example.com/trailer",
            ReleaseDate = new DateTime(2026, 1, 1),
            Rating = 8,
            Duration = TimeSpan.FromMinutes(120),
            PosterURL = "https://example.com/poster.jpg"
        };
        var session = new Session
        {
            Movie = movie,
            Hall = hall,
            Date = DateTime.Today.AddDays(1),
            StartTime = new TimeSpan(18, 0, 0),
            EndTime = new TimeSpan(20, 30, 0),
            TicketPrice = 150
        };

        var role = new Role { Name = "User", NormalizedName = "USER" };
        var user = new User
        {
            UserName = "contract_user",
            NormalizedUserName = "CONTRACT_USER",
            Email = "contract@example.com",
            NormalizedEmail = "CONTRACT@EXAMPLE.COM",
            PasswordHash = "seeded-not-a-real-hash",
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            Role = role
        };
        var payment = new Payment { User = user, Amount = 150, Date = DateTime.Today };

        context.AddRange(role, hall, movie, session, user, payment);
        await context.SaveChangesAsync();

        SeededHallId = hall.HallId;
        SeededSessionId = session.SessionId;
        SeededUserId = user.Id;
        SeededPaymentId = payment.PaymentId;
    }

    public override async ValueTask DisposeAsync()
    {
        await _mySql.DisposeAsync();
        await base.DisposeAsync();
    }
}

public sealed class TicketBookingContractTests : IClassFixture<TicketApiFactory>
{
    private readonly TicketApiFactory _factory;

    public TicketBookingContractTests(TicketApiFactory factory)
    {
        _factory = factory;
    }

    private TicketDTO TestValidBookingRequest() => new()
    {
        UserId = _factory.SeededUserId,
        UserName = "Contract User",
        SessionId = _factory.SeededSessionId,
        SessionDate = DateTime.Today.AddDays(1),
        SessionStartTime = new TimeSpan(18, 0, 0),
        HallId = _factory.SeededHallId,
        HallName = "Contract Hall",
        SeatRow = 5,
        TicketPrice = 150,
        PaymentId = _factory.SeededPaymentId,
        PaymentAmount = 150
    };

    [Fact]
    public async Task PostTicket_WithValidBooking_AcceptsRequestAndPersistsTheTicket()
    {
        var client = _factory.CreateClient();
        var request = TestValidBookingRequest();

        var response = await client.PostAsJsonAsync(
            "/api/tickets", request, TestContext.Current.CancellationToken);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(
            TestContext.Current.CancellationToken);
        Assert.Equal("Ticket booked successfully", body.GetProperty("message").GetString());
        
        var stored = await client.GetFromJsonAsync<List<TicketDTO>>(
            $"/api/tickets/user/{_factory.SeededUserId}", TestContext.Current.CancellationToken);

        Assert.NotNull(stored);
        Assert.Contains(stored!, t =>
            t.SessionId == _factory.SeededSessionId && t.SeatRow == 5);
    }

    [Fact]
    public async Task PostTicket_WithWronglyTypedField_IsRejectedAsBadRequest()
    {
        var client = _factory.CreateClient();
        var malformed = new StringContent(
            """{ "userId": 1, "seatRow": "not-a-number" }""",
            Encoding.UTF8,
            "application/json");

        var response = await client.PostAsync(
            "/api/tickets", malformed, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostTicket_WithNullBody_IsRejectedAsBadRequest()
    {
        var client = _factory.CreateClient();
        var nullBody = new StringContent("null", Encoding.UTF8, "application/json");

        var response = await client.PostAsync(
            "/api/tickets", nullBody, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
