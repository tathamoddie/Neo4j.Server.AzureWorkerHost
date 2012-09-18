using Microsoft.WindowsAzure.ServiceRuntime;

namespace AzureWorkerHost
{
    public class RoleEnvironmentWrapper : IRoleEnvironment
    {
        public LocalResource GetLocalResource(string localResourceName)
        {
            return RoleEnvironment.GetLocalResource(localResourceName);
        }
    }
}
