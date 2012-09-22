namespace Neo4j.Server.AzureWorkerHost.AzureMocks
{
    public interface ILocalResource
    {
        /// <summary>
        /// Gets the maximum size in megabytes allocated for the local storage resource, as defined in the service definition file.
        /// </summary>
        int MaximumSizeInMegabytes { get; }

        /// <summary>
        /// Gets the name of the local store as declared in the service definition file.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the full directory path to the local storage resource.
        /// </summary>
        string RootPath { get; }
    }
}
