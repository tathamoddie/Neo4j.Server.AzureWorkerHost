using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Abstractions;
using System.Text.RegularExpressions;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using Neo4j.Server.AzureWorkerHost.AzureMocks;
using Neo4j.Server.AzureWorkerHost.Diagnostics;
using Neo4j.Server.AzureWorkerHost.Legacy;

namespace Neo4j.Server.AzureWorkerHost
{
    public class NeoServer
    {
        readonly NeoServerConfiguration configuration;
        readonly IRoleEnvironment roleEnvironment;
        readonly ICloudBlobClient cloudBlobClient;
        readonly IFileSystem fileSystem;
        readonly IZipHandler zipHandler;

        internal readonly NeoRuntimeContext Context = new NeoRuntimeContext();

        public IList<ILogger> Loggers { get; private set; }

        public NeoServer(
            NeoServerConfiguration configuration,
            IRoleEnvironment roleEnvironment,
            ICloudBlobClient cloudBlobClient,
            IFileSystem fileSystem,
            IZipHandler zipHandler)
        {
            this.configuration = configuration;
            this.roleEnvironment = roleEnvironment;
            this.cloudBlobClient = cloudBlobClient;
            this.fileSystem = fileSystem;
            this.zipHandler = zipHandler;

            Loggers = new List<ILogger>(new[] { new TraceLogger() });
        }

        public NeoServer(NeoServerConfiguration configuration, CloudStorageAccount storageAccount)
            : this(configuration,
                new RoleEnvironmentWrapper(),
                new CloudBlobClientWrapper(storageAccount.CreateCloudBlobClient()),
                new FileSystem(),
                new ZipHandler())
        {}

        public NeoServer(CloudStorageAccount storageAccount)
            : this(
                new NeoServerConfiguration(),
                storageAccount)
        {}

        public void DownloadAndInstall()
        {
            InitializeLocalResource();
            DownloadJava();
            InterrogateJavaArtifact();
            DownloadNeo();
            InterrogateNeoArtifact();
            ApplyWorkaroundForJavaResolutionIssue();
            InitializeEndpoint();
            ApplyEndpointConfiguration();
        }

        public void Start()
        {
            LaunchNeoProcess();
        }

        public void Stop()
        {
            StopNeoProcess();
        }

        internal void InitializeLocalResource()
        {
            Loggers.WriteLine("Initializing local resource: {0}", configuration.NeoLocalResourceName);

            ILocalResource localResource;
            try
            {
                localResource = roleEnvironment.GetLocalResource(configuration.NeoLocalResourceName);
            }
            catch (RoleEnvironmentException ex)
            {
                var exceptionToThrow = new ApplicationException(
                    string.Format(ExceptionMessages.NeoLocalResourceNotFound, configuration.NeoLocalResourceName),
                    ex);
                Loggers.Fail(exceptionToThrow, "Local resource initialization failed");
                throw exceptionToThrow;
            }
            Context.LocalResourcePath = localResource.RootPath;
            Loggers.WriteLine("Local resource path for '{0}' is: {1}", configuration.NeoLocalResourceName, Context.LocalResourcePath);
        }

        internal void DownloadJava()
        {
            Context.JavaDirectoryPath = Path.Combine(Context.LocalResourcePath, configuration.JavaDirectoryName);
            DownloadArtifact(
                "Java Runtime Environment",
                ExceptionMessages.JavaArtifactPreparationHint,
                configuration.JavaBlobName,
                Context.JavaDirectoryPath);
        }

        internal void InterrogateJavaArtifact()
        {
            Context.JavaHomePath = Path.Combine(Context.JavaDirectoryPath, configuration.JavaHomeRelativePath);

            const string javaExePathRelativeToJavaHome = @"bin\java.exe";
            var javaExePath = Path.Combine(Context.JavaHomePath, javaExePathRelativeToJavaHome);
            if (!fileSystem.File.Exists(javaExePath))
                throw new ApplicationException(string.Format(
                    ExceptionMessages.JavaExeNotFound,
                    Path.Combine(configuration.JavaHomeRelativePath, javaExePathRelativeToJavaHome),
                    javaExePath));

            Loggers.WriteLine("java.exe found at {0}", javaExePath);
        }

        internal void DownloadNeo()
        {
            Context.NeoDirectoryPath = Path.Combine(Context.LocalResourcePath, configuration.NeoDirectoryName);
            DownloadArtifact(
                "Neo4j",
                ExceptionMessages.NeoArtifactPreparationHint,
                configuration.NeoBlobName,
                Context.NeoDirectoryPath);
        }

        internal void InterrogateNeoArtifact()
        {
            Context.NeoBasePath = Path.Combine(Context.NeoDirectoryPath, configuration.NeoBasePath);

            const string neoBatRelativePath = @"bin\neo4j.bat";
            var neoBatPath = Path.Combine(Context.NeoBasePath, neoBatRelativePath);
            if (!fileSystem.File.Exists(neoBatPath))
                throw new ApplicationException(string.Format(
                    ExceptionMessages.NeoBatNotFound,
                    neoBatRelativePath,
                    neoBatPath));

            Context.NeoBatPath = neoBatPath;

            Loggers.WriteLine("neo4j.bat found at {0}", neoBatPath);
        }

        internal void ApplyWorkaroundForJavaResolutionIssue()
        {
            // https://github.com/neo4j/packaging/issues/3

            Loggers.WriteLine("Applying workaround for Neo4j issue https://github.com/neo4j/packaging/issues/3");

            var baseBatPath = Path.Combine(Context.NeoBasePath, @"bin\base.bat");
            if (!fileSystem.File.Exists(baseBatPath))
            {
                Loggers.Fail("Couldn't find base.bat on disk; skipping patching; path was: " + baseBatPath);
                return;
            }

            var fileContents = fileSystem.File.ReadAllText(baseBatPath);
            fileContents = Regex.Replace(fileContents, "(?mi:^java )", "\"%javaPath%\\bin\\java.exe\" ");
            fileSystem.File.WriteAllText(baseBatPath, fileContents);

            Loggers.WriteLine("Patched " + baseBatPath);
        }

        internal void ApplyEndpointConfiguration()
        {
            Loggers.WriteLine("Applying endpoint configuration");

            const string configPathRelativeToNeoDirectory = @"conf\neo4j-server.properties";
            var configPath = Path.Combine(Context.NeoBasePath, configPathRelativeToNeoDirectory);
            if (!fileSystem.File.Exists(configPath))
                throw new ApplicationException(string.Format(
                    ExceptionMessages.NeoServerConfigNotFound,
                    configPathRelativeToNeoDirectory,
                    configPath
                ));

            var fileContents = fileSystem.File.ReadAllText(configPath);
            fileContents = Regex.Replace(fileContents,
                "(?mi:^[#]?org.neo4j.server.webserver.address=[^\\r]*)",
                "org.neo4j.server.webserver.address=" + Context.NeoEndpoint.Address);
            fileContents = Regex.Replace(fileContents,
                "(?mi:^[#]?org.neo4j.server.webserver.port=[^\\r]*)",
                "org.neo4j.server.webserver.port=" + Context.NeoEndpoint.Port);
            fileContents = Regex.Replace(fileContents,
                "(?mi:^[#]?org.neo4j.server.webserver.https.enabled=[^\\r]*)",
                "org.neo4j.server.webserver.https.enabled=false");
            fileSystem.File.WriteAllText(configPath, fileContents);

            Loggers.WriteLine("Patched " + configPath);
        }

        internal void DownloadArtifact(
            string friendlyName,
            string artifactPreparationHint,
            string blobName,
            string targetDirectoryPath)
        {
            Loggers.WriteLine("Downloading {0} from {1}", friendlyName, blobName);
            var blobAddress = cloudBlobClient.BaseUri.Append(blobName).AbsoluteUri;
            Loggers.WriteLine("Full blob URI is {0}", blobAddress);
            
            var blob = cloudBlobClient.GetBlobReference(blobAddress);

            var fileNameComponent = Path.GetFileName(blob.Uri.LocalPath);
            if (fileNameComponent == null)
                throw new InvalidOperationException(string.Format(
                    ExceptionMessages.PathMissingFileNameComponent,
                    blob.Uri));

            var filePathOnDisk = Path.Combine(Context.LocalResourcePath, fileNameComponent);
            Loggers.WriteLine("File path on disk will be {0}", filePathOnDisk);

            if (fileSystem.File.Exists(filePathOnDisk))
            {
                Loggers.WriteLine("File exists; deleting");
                fileSystem.File.Delete(filePathOnDisk);
            }

            Loggers.WriteLine("Downloading {0} to disk", friendlyName);
            try
            {
                blob.DownloadToFile(filePathOnDisk);
            }
            catch (StorageClientException ex)
            {
                var exceptionToThrow = new ApplicationException(
                    string.Format(ExceptionMessages.ArtifactBlobDownloadFailed, friendlyName, blobAddress, artifactPreparationHint),
                    ex);
                Loggers.Fail(exceptionToThrow, "Failed to download {0}", friendlyName);
                throw exceptionToThrow;
            }
            Loggers.WriteLine("Downloaded {0} to disk", friendlyName);

            Loggers.WriteLine("Unzipping artifact to {0}", targetDirectoryPath);
            try
            {
                zipHandler.Extract(filePathOnDisk, targetDirectoryPath);
            }
            catch (PathTooLongException ex)
            {
                throw new ApplicationException(
                    string.Format(ExceptionMessages.PathTooLongWhileUnzipping, Context.LocalResourcePath.Length),
                    ex);
            }
            Loggers.WriteLine("Unzipped artifact to {0}", targetDirectoryPath);
        }

        internal void InitializeEndpoint()
        {
            var endpoints = roleEnvironment.CurrentRoleInstance.InstanceEndpoints;
            if (!endpoints.ContainsKey(configuration.NeoEndpointId))
                throw new ArgumentException(string.Format(
                    ExceptionMessages.NeoEndpointNotFound,
                    configuration.NeoEndpointId));

            Context.NeoEndpoint = endpoints[configuration.NeoEndpointId].IPEndpoint;

            Loggers.WriteLine("Local endpoint is: {0}", Context.NeoEndpoint);
        }

        internal void LaunchNeoProcess()
        {
            var startInfo = new ProcessStartInfo(Context.NeoBatPath)
            {
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden
            };
            startInfo.EnvironmentVariables.Add("JAVA_HOME", Context.JavaHomePath);
            var neoProcess = Context.NeoProcess = new Process
            {
                StartInfo = startInfo
            };

            neoProcess.Exited += (sender, e) => Loggers.WriteLine("Neo4j process exited");
            neoProcess.ErrorDataReceived += (sender, e) => Loggers.Fail("Neo4j: " + e.Data);
            neoProcess.OutputDataReceived += (sender, e) => Loggers.WriteLine("Neo4j: " + e.Data);

            neoProcess.Start();
            neoProcess.BeginOutputReadLine();
            neoProcess.BeginErrorReadLine();
        }

        internal void StopNeoProcess()
        {
            var neoProcess = Context.NeoProcess;
            neoProcess.StandardInput.Close();
            neoProcess.WaitForExit((int)TimeSpan.FromMinutes(10).TotalMilliseconds);
        }
    }
}
