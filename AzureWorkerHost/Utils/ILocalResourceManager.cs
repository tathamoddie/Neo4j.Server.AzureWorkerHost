using Microsoft.WindowsAzure.ServiceRuntime;

namespace AzureWorkerHost.Utils
{
    internal interface ILocalResourceManager
    {
        LocalResource GetLocalResource(object localNeo4JInstallation);
    }
}