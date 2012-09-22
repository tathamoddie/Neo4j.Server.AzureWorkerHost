using System.IO;

namespace AzureWorkerHost.Legacy
{
    public interface IFileManipulation
    {
        void ReplaceConfigLine(FileInfo fileToRead, params Replacement[] replacements);
        void ReplaceTextInConfigLine(FileInfo fileToRead, string oldValue, params Replacement[] replacements);
        void AddTextToConfigLine(FileInfo fileToRead, string configLine);
    }
}