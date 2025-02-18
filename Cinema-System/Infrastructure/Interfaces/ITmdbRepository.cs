namespace Infrastructure.Interfaces;

public interface ITmdbRepository
{ 
    Task<T?> FetchFromTmdbAsync<T>(string path);
}