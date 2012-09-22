namespace Neo4j.Server.AzureWorkerHost.Diagnostics
{
    public interface ILogger
    {
        void Fail(string message);
        void WriteLine(string message);
    }
}
