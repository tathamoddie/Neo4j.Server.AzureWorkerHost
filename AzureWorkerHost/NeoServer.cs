using System;
using System.IO;
using System.IO.Abstractions;
using AzureWorkerHost.AzureMocks;
using AzureWorkerHost.Legacy;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;

namespace AzureWorkerHost
{
    public class NeoServer
    {
        readonly NeoServerConfiguration configuration;
        readonly IRoleEnvironment roleEnvironment;
        readonly ICloudBlobClient cloudBlobClient;
        readonly IFileSystem fileSystem;

        internal readonly NeoRuntimeContext Context = new NeoRuntimeContext();

        public NeoServer(
            NeoServerConfiguration configuration,
            IRoleEnvironment roleEnvironment,
            ICloudBlobClient cloudBlobClient,
            IFileSystem fileSystem)
        {
            this.configuration = configuration;
            this.roleEnvironment = roleEnvironment;
            this.cloudBlobClient = cloudBlobClient;
            this.fileSystem = fileSystem;
        }

        public NeoServer(NeoServerConfiguration configuration, CloudStorageAccount storageAccount)
            : this(configuration,
                new RoleEnvironmentWrapper(),
                new CloudBlobClientWrapper(storageAccount.CreateCloudBlobClient()),
                new FileSystem())
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
        }

        internal void InitializeLocalResource()
        {
            ILocalResource localResource;
            try
            {
                localResource = roleEnvironment.GetLocalResource(configuration.NeoLocalResourceName);
            }
            catch (RoleEnvironmentException ex)
            {
                throw new ApplicationException(
                    string.Format(ExceptionMessages.NeoLocalResourceNotFound, configuration.NeoLocalResourceName),
                    ex);
            }
            Context.LocalResourcePath = localResource.RootPath;
        }

        internal void DownloadJava()
        {
            DownloadArtifact(
                "Java Runtime Environment",
                configuration.JavaBlobName);
        }

        internal void DownloadArtifact(
            string friendlyName,
            string blobName)
        {
            var blobAddress = cloudBlobClient.BaseUri.Append(blobName);
            var blob = cloudBlobClient.GetBlobReference(blobAddress.AbsoluteUri);

            var fileNameComponent = Path.GetFileName(blob.Uri.LocalPath);
            if (fileNameComponent == null)
                throw new InvalidOperationException(string.Format(
                    ExceptionMessages.PathMissingFileNameComponent,
                    blob.Uri));

            var pathOnDisk = Path.Combine(Context.LocalResourcePath, fileNameComponent);

            if (fileSystem.File.Exists(pathOnDisk))
                fileSystem.File.Delete(pathOnDisk);

            blob.DownloadToFile(pathOnDisk);
        }
    }
}
