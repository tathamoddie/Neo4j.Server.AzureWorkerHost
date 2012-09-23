namespace Neo4j.Server.AzureWorkerHost
{
    internal static class ExceptionMessages
    {
        public const string NeoLocalResourceNotFound = "We looked for an Azure local resource named '{0}', but the Azure role environment could not supply it. This local resource is required to place the Neo4j instance into. You should configure it by placing a line like <LocalStorage name=\"{0}\" cleanOnRoleRecycle=\"true\" sizeInMB=\"500\" /> into the relevant area of your ServiceDefinition.csdef. The original exception returned by the Azure role environment is accessible via the InnerException property.";

        public const string PathMissingFileNameComponent = "We tried, but failed, to extract a file name from the URI '{0}'. The URI should end with something like /foo/bar.zip so that we can extract bar.zip as the file name component.";

        public const string ArtifactBlobDownloadFailed = "We tried, but failed, to download {0} from blob storage.\r\n\r\nThe blob URI we attempted was: {1}\r\n\r\n{2}\r\n\r\nThe original exception returned by Azure is accessible via the InnerException property.";

        public const string JavaArtifactPreparationHint = @"You need to:

1) download and install JRE7 on a development machine (just one, once)
2) ZIP the contents of c:\Program Files (x86)\Java\jre7\ to a file called jre7.zip
3) upload the result to the blob location";

        public const string NeoArtifactPreparationHint = @"You need to:

1) download http://download.neo4j.org/artifact?edition=community&version=1.8.RC1&distribution=zip
2) upload the file to the blob location";

        public const string JavaExeNotFound = @"After downloading and unzipping the Java blob, we expected but failed to find java.exe at {0}.

On disk, this path corresponds to: {1}";

        public const string NeoEndpointNotFound = "We looked for an Azure endpoint named '{0}', but the Azure role environment could not supply it. This endpoint is required for Neo4j to listen on. You should configure it by placing a line like <InternalEndpoint name=\"{0}\" protocol=\"tcp\" /> into the relevant area of your ServiceDefinition.csdef.";

        public const string PathTooLongWhileUnzipping = @"While unzipping one of the artifacts, we encountered a PathTooLongException. This generally happens during local development because the base path that the Azure emulator assigns for local resources is already really long and doesn't leave us much room to work with. (In this case, they gobbled up {0} path characters.)

To work around this:

1) set the _CSRUN_STATE_DIRECTORY environment variable on your dev workstation to something shorter like C:\AzureTemp
2) restart the Azure Compute Emulator (dev fabric)
3) restart Visual Studio

Here's a bit of PowerShell to do that for you (run as admin):

[Environment]::SetEnvironmentVariable(""_CSRUN_STATE_DIRECTORY"", ""C:\AzureTemp"", [EnvironmentVariableTarget]::User)
& ""$Env:ProgramFiles\Microsoft SDKs\Windows Azure\Emulator\csrun.exe"" /devfabric:shutdown
Get-Process devenv -ErrorAction SilentlyContinue | Stop-Process
";

        public const string NeoBatNotFound = @"After downloading and unzipping the Neo4j distribution, we expected but failed to find neo4j.bat at {0}.

On disk, this path corresponds to: {1}";

        public const string NeoServerConfigNotFound = @"After downloading and unzipping the Neo4j distribution, we expected but failed to find neo4j-server.properties at {0}.

On disk, this path corresponds to: {1}";
    }
}
