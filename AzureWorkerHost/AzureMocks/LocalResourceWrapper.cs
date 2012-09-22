using Microsoft.WindowsAzure.ServiceRuntime;

namespace Neo4j.Server.AzureWorkerHost.AzureMocks
{
    public class LocalResourceWrapper : ILocalResource
    {
        readonly LocalResource resource;

        public LocalResourceWrapper(LocalResource resource)
        {
            this.resource = resource;
        }

        public int MaximumSizeInMegabytes { get { return resource.MaximumSizeInMegabytes; } }
        public string Name { get { return resource.Name; } }
        public string RootPath { get { return resource.RootPath; } }
    }
}
