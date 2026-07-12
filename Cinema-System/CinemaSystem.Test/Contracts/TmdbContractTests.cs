using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Cinema_System.DTOs;
using Infrastructure.Repositories;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Moq;
using FluentAssertions;

namespace CinemaSystem.Test.Contracts;

public class TmdbContractTests : IAsyncLifetime
{
    // null-forgiving опер ! бо ініціалізація  в методі InitializeAsync перед кожним тестом
    private WireMockServer _server = null!;
    private TmdbRepository _repository = null!;

    public Task InitializeAsync()
    {
        _server = WireMockServer.Start();
        
        var httpClient = new HttpClient { BaseAddress = new Uri(_server.Urls[0]) };
        
        // MockBehavior.Strict тест впаде, якщо репо спробує прочитати незадекларовані налаштування з appsettings.json
        var configMock = new Mock<IConfiguration>(MockBehavior.Strict);
        configMock.Setup(c => c["TMDb:ApiKey"]).Returns("fake_api_key");
        configMock.Setup(c => c["Tmdb:Bearer"]).Returns("fake_bearer_token");
        
        // ініціаліз тест клієнта і мок конф
        _repository = new TmdbRepository(httpClient, configMock.Object);
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        _server.Stop();
        _server.Dispose();
        return Task.CompletedTask;
    }

    [Fact]
    public async Task FetchFromTmdbAsync_ValidMovieContractResponse_CorrectlyParsesMovieSearchItemDto()
    {
        // Arrange контракт у джсон
        var tmdbJsonResponse = @"
        {
            ""id"": 550,
            ""title"": ""Fight Club"",
            ""overview"": ""A ticking-time-bomb insomniac...""
        }";

        // WireMock, при GET-запиті на вказаний ендпоінт повернути 200 та джсон контракт
        _server
            .Given(Request.Create().WithPath("/3/movie/550").UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBody(tmdbJsonResponse));

        // Act запит через репо на фейк сервер
        var result = await _repository.FetchFromTmdbAsync<MovieSearchItemDTO>("/3/movie/550"); // Прибрав зайвий '?' для стабільності

        // Assert чек десеріалізацію в дто
        result.Should().NotBeNull();
        result!.Id.Should().Be(550);
        result.Title.Should().Be("Fight Club");
        result.Overview.Should().Be("A ticking-time-bomb insomniac...");
    }
}