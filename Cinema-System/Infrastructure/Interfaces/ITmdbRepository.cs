namespace Infrastructure.Interfaces;

public interface ITmdbRepository
{
    public interface ITmdbRepository
    {
        Task<T?> FetchFromTmdbAsync<T>(string relativePath);
    }
}