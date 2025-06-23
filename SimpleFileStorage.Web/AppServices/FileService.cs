using Microsoft.Extensions.Options;
using SimpleFileStorage.Web.AppServices.Contract;
using SimpleFileStorage.Web.AppServices.Contract.Response;
using SimpleFileStorage.Web.DataAccess.Contract;
using SimpleFileStorage.Web.Domain;
using SimpleFileStorage.Web.Infrastructure;

namespace SimpleFileStorage.Web.AppServices;

public sealed class FileService : IFileService
{
    private readonly IFileRepository _fileRepository;
    private readonly IFileSystemRepository _fsRepository;
    private readonly StorageSettings _settings;

    public FileService(IFileRepository fileRepository, IFileSystemRepository fsRepository, IOptions<StorageSettings> settings)
    {
        _fileRepository = fileRepository;
        _fsRepository = fsRepository;
        _settings = settings.Value;
    }

    public async Task<StoredFile> UploadAsync(IFormFile file, string fileName)
    {
        if (file.Length > _settings.MaxFileSizeKb * 1024)
            throw new InvalidOperationException("File too large");

        await _fsRepository.SaveAsync(file, fileName);

        var stored = new StoredFile
        {
            Id = Guid.CreateVersion7(),
            FileName = fileName,
            UploadedAt = DateTime.UtcNow
        };

        return await _fileRepository.AddAsync(stored);
    }

    public async Task<DownloadResponse> DownloadAsync(Guid id)
    {
        var file = await _fileRepository.GetAsync(id) ?? throw new FileNotFoundException();
        return new DownloadResponse
        {
            File = await _fsRepository.ReadAsync(file.FileName),
            FileName = file.FileName
        };
    }

    public async Task DeleteAsync(Guid id)
    {
        var file = await _fileRepository.GetAsync(id) ?? throw new FileNotFoundException();
        _fsRepository.Delete(file.FileName);
        await _fileRepository.DeleteAsync(file);
    }

    public Task<List<StoredFile>> GetListAsync(int take, int skip, string? searchTerm) => _fileRepository.ListAsync(take, skip, searchTerm);
}
