namespace AzureWorkerHost
{
    public static class ExceptionMessages
    {
        public const string NeoLocalResourceNotFound = "We looked for an Azure local resource named '{0}', but the Azure role environment could not supply it. This local resource is required to place the Neo4j instance into. You should configure it by placing a line like <LocalStorage name=\"{0}\" cleanOnRoleRecycle=\"true\" sizeInMB=\"500\" /> into the relevant area of your ServiceDefinition.csdef. The original exception returned by the Azure role environment is accessible via the InnerException property.";

        public const string PathMissingFileNameComponent = "We tried, but failed, to extract a file name from the URI '{0}'. The URI should end with something like /foo/bar.zip so that we can extract bar.zip as the file name component.";

        public const string ArtifactBlobDownloadFailed = "We tried, but failed, to download {0} from blob storage.\r\n\r\nThe blob URI we attempted was: {1}\r\n\r\n{2}\r\n\r\nThe original exception returned by Azure is accessible via the InnerException property.";

        public const string JavaArtifactPreparationHint = @"You need to:

1) download and install JRE7 on a development machine (just one, once)
2) ZIP the contents of c:\Program Files (x86)\Java\jre7\ to a file called jre7.zip
3) upload the result to the blob location";
    }
}
