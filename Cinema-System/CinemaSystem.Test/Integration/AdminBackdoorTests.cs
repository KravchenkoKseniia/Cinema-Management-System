using System.Net;
using Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaSystem.Test.Integration;
public sealed class SecurityAppFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName = $"security-tests-{Guid.NewGuid()}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var toRemove = services.Where(d =>
                    d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>) ||
                    d.ServiceType == typeof(ApplicationDbContext) ||
                    (d.ServiceType.IsGenericType &&
                     d.ServiceType.GetGenericTypeDefinition().Name.StartsWith("IDbContextOptionsConfiguration")))
                .ToList();

            foreach (var descriptor in toRemove)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(_databaseName));
        });
    }
}

public sealed class AdminBackdoorTests : IClassFixture<SecurityAppFactory>
{
    private readonly SecurityAppFactory _factory;

    public AdminBackdoorTests(SecurityAppFactory factory)
    {
        _factory = factory;
    }

    private HttpClient CreateClientThatDoesNotFollowRedirects() =>
        _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

    private static FormUrlEncodedContent LoginForm(string userName, string password) =>
        new(new Dictionary<string, string>
        {
            ["UserName"] = userName,
            ["Password"] = password
        });

    [Fact]
    public async Task Login_WithHardcodedAdminCredentials_BypassesAuthenticationAndRedirectsToSessions()
    {
        // AuthorizationUIController.Login contains a hardcoded master credential (admin / passwordPassword123@@@)
        var client = CreateClientThatDoesNotFollowRedirects();

        var response = await client.PostAsync(
            "/AuthorizationUI/Login",
            LoginForm("admin", "passwordPassword123@@@"),
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.NotNull(response.Headers.Location);
        Assert.Contains("SessionsUI", response.Headers.Location!.ToString());
    }

    [Fact]
    public async Task Login_WithAdminUsernameButAnyOtherPassword_DoesNotGetTheBypass()
    {
        var client = CreateClientThatDoesNotFollowRedirects();

        var response = await client.PostAsync(
            "/AuthorizationUI/Login",
            LoginForm("admin", "not-the-magic-password"),
            TestContext.Current.CancellationToken);

        var location = response.Headers.Location?.ToString() ?? string.Empty;
        Assert.DoesNotContain("SessionsUI", location);
    }
}
