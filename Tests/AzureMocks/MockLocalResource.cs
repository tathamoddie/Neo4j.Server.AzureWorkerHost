using Neo4j.Server.AzureWorkerHost.AzureMocks;

namespace Tests.AzureMocks
{
    public class MockLocalResource : ILocalResource
    {
        public int MaximumSizeInMegabytes { get; set; }
        public string Name { get; set; }
        public string RootPath { get; set; }
    }
}
