using Microsoft.WindowsAzure.ServiceRuntime;

namespace AzureWorkerHost.Legacy
{
    internal interface ILocalResourceManager
    {
        LocalResource GetLocalResource(object localNeo4JInstallation);
    }
}