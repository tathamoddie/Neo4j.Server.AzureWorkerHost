using System.Collections.Generic;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace Neo4j.Server.AzureWorkerHost.AzureMocks
{
    public class RoleInstanceWrapper : IRoleInstance
    {
        readonly RoleInstance roleInstance;

        public RoleInstanceWrapper(RoleInstance roleInstance)
        {
            this.roleInstance = roleInstance;
        }

        public IDictionary<string, RoleInstanceEndpoint> InstanceEndpoints { get { return roleInstance.InstanceEndpoints; } }
    }
}
