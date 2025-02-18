using System.Net.Http.Json;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories
{
    public class TmdbRepository : ITmdbRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _bearerToken;

        public TmdbRepository(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["TMDb:ApiKey"] ?? throw new ArgumentNullException("API key is missing!");
            _bearerToken = configuration["Tmdb:Bearer"] ?? throw new ArgumentNullException("Bearer token is missing!");

            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _bearerToken);
        }

        public async Task<T?> FetchFromTmdbAsync<T>(string path)
        {
            var url = $"{path}&api_key={_apiKey}";
            return await _httpClient.GetFromJsonAsync<T>(url);
        }
    }
}