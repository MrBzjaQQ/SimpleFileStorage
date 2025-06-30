using SimpleFileStorage.Web.AppServices.Contract;

namespace SimpleFileStorage.Web.AppServices;

public class GuidProvider: IGuidProvider
{
    public Guid CreateVersion7()
    {
        return Guid.CreateVersion7();
    }
}