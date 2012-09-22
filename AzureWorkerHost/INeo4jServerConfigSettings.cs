namespace AzureWorkerHost
{
    public interface INeo4JServerConfigSettings
    {
        string Port { get; }
        string IpAddress { get; }
        string WebAdminDataUri { get; }
        string WebAdminManagementUri { get; }
        string DatabaseLocation { get; }
    }
}