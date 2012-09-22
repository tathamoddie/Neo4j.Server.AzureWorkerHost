using Microsoft.WindowsAzure.ServiceRuntime;

namespace Neo4j.Server.AzureWorkerHost.Legacy
{
    internal interface ILocalResourceManager
    {
        LocalResource GetLocalResource(object localNeo4JInstallation);
    }
}