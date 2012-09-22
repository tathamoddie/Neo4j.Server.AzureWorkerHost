namespace AzureWorkerHost.Legacy
{
    public interface IZipping
    {
        void Extract(string fullName, string directoryName);
    }
}