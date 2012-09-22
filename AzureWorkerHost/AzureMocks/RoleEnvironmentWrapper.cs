using Microsoft.WindowsAzure.ServiceRuntime;

namespace AzureWorkerHost.AzureMocks
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
