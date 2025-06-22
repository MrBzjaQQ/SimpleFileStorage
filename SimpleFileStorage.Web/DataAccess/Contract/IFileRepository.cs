using SimpleFileStorage.Web.Domain;

namespace SimpleFileStorage.Web.DataAccess.Contract;

public interface IFileRepository
{
    Task<StoredFile> AddAsync(StoredFile file);
    Task<StoredFile?> GetAsync(Guid id);
    Task<List<StoredFile>> ListAsync(int take, int skip, string? searchTerm);
    Task DeleteAsync(StoredFile file);
}