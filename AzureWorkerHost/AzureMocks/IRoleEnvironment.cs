namespace Neo4j.Server.AzureWorkerHost.AzureMocks
{
    public interface IRoleEnvironment
    {
        ILocalResource GetLocalResource(string localResourceName);
        IRoleInstance CurrentRoleInstance { get; }
    }
}
