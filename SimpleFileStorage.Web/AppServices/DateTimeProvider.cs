using SimpleFileStorage.Web.AppServices.Contract;

namespace SimpleFileStorage.Web.AppServices;

public class DateTimeProvider: IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}