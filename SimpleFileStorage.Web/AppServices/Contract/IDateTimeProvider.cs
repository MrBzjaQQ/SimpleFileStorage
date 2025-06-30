namespace SimpleFileStorage.Web.AppServices.Contract;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}