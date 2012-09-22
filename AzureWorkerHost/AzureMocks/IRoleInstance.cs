using System.Collections.Generic;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace Neo4j.Server.AzureWorkerHost.AzureMocks
{
    public interface IRoleInstance
    {
        IDictionary<string, RoleInstanceEndpoint> InstanceEndpoints { get; }
    }
}
