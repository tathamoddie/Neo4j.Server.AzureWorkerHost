namespace AzureWorkerHost
{
    public interface IZipping
    {
        void Extract(string fullName, string directoryName);
    }
}