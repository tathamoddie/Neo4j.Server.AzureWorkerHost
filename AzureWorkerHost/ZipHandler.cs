using Ionic.Zip;

namespace AzureWorkerHost
{
    public class ZipHandler : IZipHandler
    {
        public void Extract(string zipFilePath, string targetDirectoryPath)
        {
            using (var zipFile = ZipFile.Read(zipFilePath))
            {
                zipFile.ExtractAll(targetDirectoryPath, ExtractExistingFileAction.OverwriteSilently);
            }
        }
    }
}
