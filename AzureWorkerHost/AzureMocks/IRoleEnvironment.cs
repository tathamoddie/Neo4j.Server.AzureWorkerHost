namespace AzureWorkerHost.AzureMocks
{
    public interface IRoleEnvironment
    {
        ILocalResource GetLocalResource(string localResourceName);
    }
}
