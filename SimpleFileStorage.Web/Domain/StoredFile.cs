namespace SimpleFileStorage.Web.Domain;

public sealed record StoredFile
{
    public required Guid Id { get; set; }
    public required string FileName { get; set; }
    public required DateTime UploadedAt { get; set; }
}