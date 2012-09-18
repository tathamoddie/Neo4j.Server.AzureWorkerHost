using Microsoft.WindowsAzure.ServiceRuntime;

namespace AzureWorkerHost
{
    public interface IRoleEnvironment
    {
        LocalResource GetLocalResource(string localResourceName);
    }
}
