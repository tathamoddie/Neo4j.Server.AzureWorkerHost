using System;
using System.IO;
using System.Net;

namespace AzureWorkerHost.Utils
{
    public interface IPaths
    {
        DirectoryInfo Neo4JLogPath { get; }
        DirectoryInfo Neo4JInstRoot { get; }

        FileInfo JavaExeFile { get; }
        FileInfo Neo4JExeFile { get; }
        FileInfo Neo4JConfigFile { get; }
        FileInfo Neo4JServerConfigFile { get; }
        FileInfo Neo4JWrapperConfigFile { get; }
        FileInfo Neo4JLoggingConfigFile { get; }

        INeo4JServerConfigSettings Neo4JServerConfigSettings { get; }

        string Neo4JWrapperSettingLogFile { get; }
        string Neo4JLogsContainerName { get; }
        string Neo4jDBPath { get; }
        string Neo4JdbVirtualHardDriveBlobName { get; }
        string NewRelicBlobRelativePath { get; }
        string NewRelicConfigFile { get; }

        int Neo4JPort { get; }
        IPAddress Neo4JIpAddress { get; }
        int? Neo4jDBDriveBlobSizeIfNotExists { get; }

        Uri GetNeo4JDataUri();
        Uri GetNeo4JAdminManagementUri();
    }
}