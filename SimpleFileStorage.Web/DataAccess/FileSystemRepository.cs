using Microsoft.Extensions.Options;
using SimpleFileStorage.Web.DataAccess.Contract;
using SimpleFileStorage.Web.Infrastructure;

namespace SimpleFileStorage.Web.DataAccess;

public sealed class FileSystemRepository : IFileSystemRepository
{
    private readonly IWebHostEnvironment _env;
    private readonly StorageSettings _settings;

    public FileSystemRepository(IWebHostEnvironment env, IOptions<StorageSettings> settings)
    {
        _env = env;
        _settings = settings.Value;
    }

    public async Task SaveAsync(IFormFile file, string path)
    {
        var fullPath = Path.Combine(_settings.FileStoragePath, path);
        var directory = Path.GetDirectoryName(fullPath);
        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory!);

        await using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);
    }

    public Task<Stream> ReadAsync(string path)
    {
        var fullPath = Path.Combine(_settings.FileStoragePath, path);
        return Task.FromResult<Stream>(File.OpenRead(fullPath));
    }

    public void Delete(string path)
    {
        var fullPath = Path.Combine(_settings.FileStoragePath, path);
        if (File.Exists(fullPath)) File.Delete(fullPath);
    }
}
