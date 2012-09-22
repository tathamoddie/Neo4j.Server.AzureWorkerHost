namespace AzureWorkerHost
{
    public interface IZipHandler
    {
        void Extract(string zipFilePath, string targetDirectoryPath);
    }
}
