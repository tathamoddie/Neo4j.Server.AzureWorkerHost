namespace Neo4j.Server.AzureWorkerHost
{
    public interface IZipHandler
    {
        void Extract(string zipFilePath, string targetDirectoryPath);
    }
}
