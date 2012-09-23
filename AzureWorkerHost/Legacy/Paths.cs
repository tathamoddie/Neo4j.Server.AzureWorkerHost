using System;
using System.Diagnostics;
using System.IO;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace Neo4j.Server.AzureWorkerHost.Legacy
{
    [Obsolete]
    internal class Paths
    {
        readonly IConfiguration configuration;
        DirectoryInfo neo4JLogPath;
        FileInfo neo4JConfigFile;
        FileInfo neo4JServerConfigFile;
        FileInfo neo4JWrapperConfigFile;
        FileInfo neo4JLoggingConfigFile;

        public Paths(
            IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public DirectoryInfo Neo4JInstRoot
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public DirectoryInfo Neo4JLogPath
        {
            get
            {
                if (neo4JLogPath == null)
                {
                    var neo4JLogFolderPath = configuration.Neo4JLogFolderPath();
                    neo4JLogPath = new DirectoryInfo(Path.Combine(Neo4JInstRoot.FullName, neo4JLogFolderPath));
                }
                return neo4JLogPath;
            }
        }

        public FileInfo Neo4JServerConfigFile
        {
            get
            {
                if (neo4JServerConfigFile == null)
                {
                    var neo4JServerConfigFileName = configuration.Neo4JServerConfigFileName();
                    neo4JServerConfigFile = Neo4JInstRoot.FindFile(neo4JServerConfigFileName);
                }
                return neo4JServerConfigFile;
            }
        }

        public FileInfo Neo4JLoggingConfigFile
        {
            get
            {
                if (neo4JLoggingConfigFile == null)
                {
                    var neo4JLoggingConfigFileName = configuration.Neo4JLoggingFileName();
                    var configFolder = Neo4JInstRoot.FindDirectoryThatEndsWith("conf");
                    neo4JLoggingConfigFile = configFolder.FindFile(neo4JLoggingConfigFileName);
                }
                return neo4JLoggingConfigFile;
            }
        }

        public FileInfo Neo4JConfigFile
        {
            get
            {
                if (neo4JConfigFile == null)
                {
                    var neo4JConfigFileName = configuration.Neo4JConfigFileName();
                    neo4JConfigFile = Neo4JInstRoot.FindFile(neo4JConfigFileName);
                }
                return neo4JConfigFile;
            }
        }

        public FileInfo Neo4JWrapperConfigFile
        {
            get
            {
                if (neo4JWrapperConfigFile == null)
                {
                    var neo4JWrapperConfigFileName = configuration.Neo4JWrapperConfigFileName();
                    neo4JWrapperConfigFile = Neo4JInstRoot.FindFile(neo4JWrapperConfigFileName);
                }
                return neo4JWrapperConfigFile;
            }
        }

        public string Neo4JWrapperSettingLogFile
        {
            get { return configuration.Neo4JWrapperSettingLogFile(); }
        }

        public string Neo4JLogsContainerName
        {
            get { return configuration.Neo4JLogsContainerName(); }
        }

        public string NewRelicConfigFile
        {
            get { return configuration.NewRelicConfigFileName(); }
        }

        public string Neo4JdbVirtualHardDriveBlobName
        {
            get { return configuration.Neo4JdbVirtualHardDriveBlobName(); }
        }

        public string NewRelicBlobRelativePath
        {
            get { return configuration.NewRelicBlobRelativePath(); }
        }

        public int? Neo4jDBDriveBlobSizeIfNotExists
        {
            get
            {
                var configurationSettingValue = "";
                var result = 0;
                try
                {
                    configurationSettingValue = configuration.Neo4JdbDriveBlobSizeIfNotExists();
                    result = Convert.ToInt32(configurationSettingValue);
                }
                catch (RoleEnvironmentException)
                {
                    Trace.TraceWarning(
                        "Unable to find a configuration setting named '{0}'. Defaulting to null (nullable int).",
                        "Neo4jDBDriveBlobSizeIfNotExists");
                }
                catch (FormatException fe)
                {
                    Trace.TraceWarning(
                        "Unable to convert the configuration setting named '{0}' to an integer value. The value was '{1}'. Error message: {2}",
                        "Neo4jDBDriveBlobSizeIfNotExists", configurationSettingValue, fe.Message);
                }
                return result;
            }
        }

        public string Neo4jDBPath
        {
            get { return @"x:\Neo"; }
        }
    }
}