using System;
using Microsoft.WindowsAzure.StorageClient;

namespace Neo4j.Server.AzureWorkerHost.AzureMocks
{
    public class CloudBlobClientWrapper : ICloudBlobClient
    {
        readonly CloudBlobClient client;

        public CloudBlobClientWrapper(CloudBlobClient client)
        {
            this.client = client;
        }

        public Uri BaseUri { get { return client.BaseUri; } }

        public CloudBlob GetBlobReference(string blobAddress)
        {
            return client.GetBlobReference(blobAddress);
        }
    }
}
