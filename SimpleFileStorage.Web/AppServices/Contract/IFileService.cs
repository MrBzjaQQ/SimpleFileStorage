using SimpleFileStorage.Web.AppServices.Contract.Response;
using SimpleFileStorage.Web.Domain;

namespace SimpleFileStorage.Web.AppServices.Contract;

public interface IFileService
{
    Task<StoredFile> UploadAsync(IFormFile file, string fileName);
    Task<DownloadResponse> DownloadAsync(Guid id);
    Task DeleteAsync(Guid id);
    Task<List<StoredFile>> GetListAsync(int take, int skip, string? searchTerm);
}