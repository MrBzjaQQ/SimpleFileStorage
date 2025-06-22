using Microsoft.EntityFrameworkCore;
using SimpleFileStorage.Web.DataAccess.Contract;
using SimpleFileStorage.Web.Domain;
using SimpleFileStorage.Web.Infrastructure;

namespace SimpleFileStorage.Web.DataAccess;

public sealed class FileRepository : IFileRepository
{
    private readonly StoredFilesContext _db;

    public FileRepository(StoredFilesContext db) => _db = db;

    public async Task<StoredFile> AddAsync(StoredFile file)
    {
        _db.StoredFiles.Add(file);
        await _db.SaveChangesAsync();
        return file;
    }

    public Task<StoredFile?> GetAsync(Guid id) => _db.StoredFiles.FindAsync(id).AsTask();

    public Task<List<StoredFile>> ListAsync(int take, int skip, string? searchTerm)
    {
        var query = _db.StoredFiles.AsQueryable();
        if (!string.IsNullOrWhiteSpace(searchTerm))
            query = query.Where(f => f.FileName.Contains(searchTerm));
        return query.OrderByDescending(f => f.UploadedAt).Skip(skip).Take(take).ToListAsync();
    }

    public async Task DeleteAsync(StoredFile file)
    {
        _db.StoredFiles.Remove(file);
        await _db.SaveChangesAsync();
    }
}