namespace AzureWorkerHost
{
    public static class ExceptionMessages
    {
        public const string NeoLocalResourceNotFound = "We looked for an Azure local resource named '{0}', but the Azure role environment could not supply it. This local resource is required to place the Neo4j instance into. You should configure it by placing a line like <LocalStorage name=\"{0}\" cleanOnRoleRecycle=\"true\" sizeInMB=\"500\" /> into the relevant area of your ServiceDefinition.csdef. The original exception returned by the Azure role environment is accessible via the InnerException property.";
    }
}
