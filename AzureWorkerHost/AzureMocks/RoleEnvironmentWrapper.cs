using Microsoft.WindowsAzure.ServiceRuntime;

namespace Neo4j.Server.AzureWorkerHost.AzureMocks
{
    public class RoleEnvironmentWrapper : IRoleEnvironment
    {
        public ILocalResource GetLocalResource(string localResourceName)
        {
            var resource = RoleEnvironment.GetLocalResource(localResourceName);
            return new LocalResourceWrapper(resource);
        }
    }
}
