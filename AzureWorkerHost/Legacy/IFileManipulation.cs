using System.IO;

namespace Neo4j.Server.AzureWorkerHost.Legacy
{
    public interface IFileManipulation
    {
        void ReplaceConfigLine(FileInfo fileToRead, params Replacement[] replacements);
        void ReplaceTextInConfigLine(FileInfo fileToRead, string oldValue, params Replacement[] replacements);
        void AddTextToConfigLine(FileInfo fileToRead, string configLine);
    }
}