namespace SimpleFileStorage.Web.Infrastructure;

public sealed record StorageSettings
{
    public int MaxFileSizeKb { get; set; }
    public string FileStoragePath { get; set; } = string.Empty;
}