namespace SimpleFileStorage.Web.AppServices.Contract.Response;

public sealed record DownloadResponse
{
    public required Stream File { get; init; }
    public required string FileName { get; init; }
}