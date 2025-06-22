using Microsoft.EntityFrameworkCore;
using SimpleFileStorage.Web.Domain;

namespace SimpleFileStorage.Web.Infrastructure;

public sealed class StoredFilesContext: DbContext
{
    public StoredFilesContext(DbContextOptions<StoredFilesContext> options) : base(options) { }

    public DbSet<StoredFile> StoredFiles => Set<StoredFile>();
}