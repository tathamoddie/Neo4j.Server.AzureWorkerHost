using System;
using Microsoft.WindowsAzure.StorageClient;

namespace AzureWorkerHost.AzureMocks
{
    public interface ICloudBlobClient
    {
        Uri BaseUri { get; }
        CloudBlob GetBlobReference(string blobAddress);
    }
}
