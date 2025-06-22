namespace SimpleFileStorage.Web.DataAccess.Contract;

public interface IFileSystemRepository
{
    Task SaveAsync(IFormFile file, string path);
    Task<Stream> ReadAsync(string path);
    void Delete(string path);
}