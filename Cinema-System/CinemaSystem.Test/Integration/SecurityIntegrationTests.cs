using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Data;
using FluentAssertions;

namespace CinemaSystem.Test.Integration;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // глушимо справжню  дб реєсртрацію
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // ін-меморі дб
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTestingSecurity");
            });
        });
    }
}

public class SecurityIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public SecurityIntegrationTests(CustomWebApplicationFactory factory)
    {
        // щоб можна було піймати 302 редирект
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task AccessProtectedTicketsEndpoint_WithoutAuthentication_ShouldRedirectToLogin()
    {
        // Arrange
        var protectedUrl = "/TicketsUI/Index";

        // Act
        var response = await _client.GetAsync(protectedUrl);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Redirect);

        var redirectLocation = response.Headers.Location?.ToString();
        redirectLocation.Should().NotBeNull();
        redirectLocation.Should().Contain("Login");
    }
}