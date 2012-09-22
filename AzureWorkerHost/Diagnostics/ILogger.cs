namespace AzureWorkerHost.Diagnostics
{
    public interface ILogger
    {
        void Fail(string message);
        void WriteLine(string message);
    }
}
