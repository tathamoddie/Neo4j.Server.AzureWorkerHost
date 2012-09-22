using System.IO;
using Microsoft.WindowsAzure;

namespace AzureWorkerHost.Legacy
{
    public interface ICloudDriveManager
    {
        void CreateIfNotExists(CloudStorageAccount cloudStorageAccount, string driveRelativePath, int value);
        DirectoryInfo Mount(CloudStorageAccount cloudStorageAccount, string driveRelativePath);
    }
}