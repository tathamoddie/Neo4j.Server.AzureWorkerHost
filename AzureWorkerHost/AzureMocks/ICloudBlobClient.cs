using System;
using Microsoft.WindowsAzure.StorageClient;

namespace Neo4j.Server.AzureWorkerHost.AzureMocks
{
    public interface ICloudBlobClient
    {
        Uri BaseUri { get; }
        CloudBlob GetBlobReference(string blobAddress);
    }
}
