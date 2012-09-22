namespace AzureWorkerHost
{
    internal class NeoRuntimeContext
    {
        /// <summary>
        /// Path on disk of the Azure local resource where Neo4j and all supporting files will be installed and run from
        /// </summary>
        public string LocalResourcePath { get; set; }
    }
}
