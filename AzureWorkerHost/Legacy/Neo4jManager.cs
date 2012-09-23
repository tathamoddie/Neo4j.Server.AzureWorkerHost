using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.WindowsAzure;

namespace Neo4j.Server.AzureWorkerHost.Legacy
{
    [Obsolete]
    internal class Neo4JManager
    {
        readonly FileManipulation fileManipulation;
        readonly Paths paths;
        readonly ICloudDriveManager cloudDriveManager;
        readonly IConfiguration configuration;

        public Neo4JManager(
            FileManipulation fileManipulation,
            Paths paths,
            ICloudDriveManager cloudDriveManager,
            IConfiguration configuration)
        {
            this.paths = paths;
            this.cloudDriveManager = cloudDriveManager;
            this.configuration = configuration;
            this.fileManipulation = fileManipulation;
        }

        public void Install()
        {
            Trace.TraceInformation("Installing Neo4j server.");

            //Ensure logging folder is created, in case diagnostics is turned off.
            Directory.CreateDirectory(Path.Combine(paths.Neo4JInstRoot.FullName, configuration.Neo4JLogFolderPath()));

            // Download
            //DownloadJava();
            //DownloadNeo4J();
            //UnzipAllZipFiles();

            // Validate
            //FindRequiredFilesAndDirectories();

            // Configure
            SetDatabaseConfiguration();
            //SetServerConfiguration();
            SetWrapperConfiguration();
            SetLoggingConfiguration();
            //PatchPathsInBaseBat();
            //AddJavaFolderToEnvironmentPathVariable(paths);
            
            // Database network drive
            var neo4JDbPath = MountDatabaseLocation();

            CheckCloudDriveIsMounted(neo4JDbPath);

            SetServerDbPathConfiguration(neo4JDbPath);
            Trace.TraceInformation("Neo4j server installed.");
        }

        static void CheckCloudDriveIsMounted(DirectoryInfo neo4JDbPath)
        {
            if (neo4JDbPath != null && neo4JDbPath.Exists)
            {
                foreach (var directories in neo4JDbPath.GetDirectories())
                {
                    Trace.WriteLine(string.Format("Neo4j DB contains directory {0}",directories.Name));
                }
            }
            else
            {
                Trace.WriteLine("Could not locate cloud drive");
            }
        }

        internal DirectoryInfo MountDatabaseLocation()
        {
            DirectoryInfo neo4JDatabaseDirectory;

            if (paths.Neo4jDBPath != null)
            {
                Trace.TraceInformation("Absolute Neoj4 database directory configured to {0}.", paths.Neo4jDBPath);
                neo4JDatabaseDirectory = new DirectoryInfo(paths.Neo4jDBPath);
            }
            else
            {
                Trace.TraceInformation("Mounting the Neoj4 database blob drive.");

                var cloudStorageAccount = CloudStorageAccount.FromConfigurationSetting(ConfigConstants.DriveConnectionString);
                var driveRelativePath = paths.Neo4JdbVirtualHardDriveBlobName;

                if (paths.Neo4jDBDriveBlobSizeIfNotExists.HasValue)
                {
                    cloudDriveManager.CreateIfNotExists(cloudStorageAccount, driveRelativePath,
                                                        paths.Neo4jDBDriveBlobSizeIfNotExists.Value);
                }

                neo4JDatabaseDirectory = cloudDriveManager.Mount(cloudStorageAccount, driveRelativePath);

                if (neo4JDatabaseDirectory.GetDirectories().All(d => d.Name != configuration.CloudDriveDatabaseMountPath()))
                {
                    neo4JDatabaseDirectory.CreateSubdirectory(configuration.CloudDriveDatabaseMountPath());
                }

                Trace.TraceInformation("Done mounting the Neoj4 database blob drive.");
            }

            return neo4JDatabaseDirectory;
        }

        internal void SetDatabaseConfiguration()
        {
            Trace.TraceInformation("Setting configurations in the file <{0}>.", paths.Neo4JConfigFile);

            fileManipulation.ReplaceConfigLine(
                paths.Neo4JConfigFile,
                Replacement.Create(
                    "#allow_store_upgrade=",
                    "allow_store_upgrade=" + configuration.Neo4JAllowStoreUpgrade()));

            Trace.TraceInformation("Done setting configurations in the file <{0}>.", paths.Neo4JConfigFile);
        }

        internal void SetWrapperConfiguration()
        {
            Trace.TraceInformation("Setting configurations in the file <{0}>.", paths.Neo4JWrapperConfigFile);
            Trace.TraceInformation("Setting log directory to '{0}'.", paths.Neo4JLogPath.FullName);

            var wrapperLogFile = paths.Neo4JWrapperSettingLogFile;
            var patternToFind = string.Format("{0}=", wrapperLogFile);
            var replaceWith = string.Format("{0}{1}", patternToFind,
                                         Path.Combine(paths.Neo4JLogPath.FullName, "wrapper.log"));

            fileManipulation.ReplaceConfigLine(paths.Neo4JWrapperConfigFile, Replacement.Create(patternToFind, replaceWith));

            var minHeapPattern = configuration.Neo4JLoggingMinHeapPattern();
            var maxHeapPattern = configuration.Neo4JLoggingMaxHeapPattern();

            var adjustedMinHeapSize = configuration.Neo4JLoggingMinHeap();
            var adjustedMaxHeapSize = configuration.Neo4JLoggingMaxHeap();

            ApplyAvailableMemoryAdjustmentToHeapSize(ref adjustedMinHeapSize, ref adjustedMaxHeapSize);

            fileManipulation.ReplaceConfigLine(paths.Neo4JWrapperConfigFile, Replacement.Create(minHeapPattern, adjustedMinHeapSize));
            fileManipulation.ReplaceConfigLine(paths.Neo4JWrapperConfigFile, Replacement.Create(maxHeapPattern, adjustedMaxHeapSize));

            var destination = Path.GetFileName(paths.Neo4JWrapperConfigFile.Name);
            if (destination == null)
                throw new ApplicationException("Neo4JWrapperConfig must be present.");

            if (configuration.Neo4JWrapperEnableGarbageCollectionLogging())
                fileManipulation.ReplaceConfigLine(paths.Neo4JWrapperConfigFile,
                    Replacement.Create("#" + configuration.Neo4JWrapperGarbageCollectionLoggingCommand(), configuration.Neo4JWrapperGarbageCollectionLoggingCommand()));

            if (Environment.Is64BitOperatingSystem)
            {
                foreach (
                    var jvmSwitch in
                        configuration.JvmSwitches().Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    fileManipulation.AddTextToConfigLine(paths.Neo4JWrapperConfigFile, jvmSwitch);
            }

            File.Copy(
                paths.Neo4JWrapperConfigFile.FullName,
                Path.Combine(paths.Neo4JLogPath.FullName, destination),
                true);

            Trace.TraceInformation("Done setting configurations in the file <{0}>.", paths.Neo4JWrapperConfigFile);
        }

        static void ApplyAvailableMemoryAdjustmentToHeapSize(ref string minMemoryHeap, ref string maxMemoryHeap)
        {
            //var computerInfo = new ComputerInfo();
            //var avaliableMemory = computerInfo.AvailablePhysicalMemory / 1024 / 1024;

            //var minMemorySize = ulong.Parse(minMemoryHeap.Split('=')[1]);
            //var maxMemorySize = ulong.Parse(maxMemoryHeap.Split('=')[1]);

            //if (minMemorySize > avaliableMemory || maxMemorySize > avaliableMemory)
            //{
            //    var adjustedMinHeapSize = (int)(avaliableMemory * (0.5));
            //    var adjustedMaxHeapSize = (int)(avaliableMemory * (0.7));

            //    if (adjustedMinHeapSize < 3)
            //    {
            //        adjustedMinHeapSize = 3;
            //        adjustedMaxHeapSize = 64;
            //    }

            //    minMemoryHeap = minMemoryHeap.Replace(minMemorySize.ToString(CultureInfo.InvariantCulture), adjustedMinHeapSize.ToString(CultureInfo.InvariantCulture));
            //    maxMemoryHeap = maxMemoryHeap.Replace(maxMemorySize.ToString(CultureInfo.InvariantCulture), adjustedMaxHeapSize.ToString(CultureInfo.InvariantCulture));
            //}
        }

        internal void SetLoggingConfiguration()
        {
            Trace.TraceInformation("Setting configurations in the file <{0}>.", paths.Neo4JLoggingConfigFile);
            Trace.TraceInformation("Setting log directory to '{0}'.", paths.Neo4JLogPath.FullName);

            // SEVERE (highest value)
            // WARNING
            // INFO
            // CONFIG
            // FINE
            // FINER
            // FINEST (lowest value)

            var patternToFind = configuration.Neo4JLoggingLevelPattern();
            var replaceWith = configuration.Neo4JLoggingLevel();
            fileManipulation.ReplaceConfigLine(paths.Neo4JLoggingConfigFile, Replacement.Create(patternToFind, replaceWith));

            var destination = Path.GetFileName(paths.Neo4JLoggingConfigFile.Name);
            if (destination == null)
                throw new ApplicationException("Neo4JLoggingConfigFile must be present.");

            File.Copy(
                paths.Neo4JLoggingConfigFile.FullName,
                Path.Combine(paths.Neo4JLogPath.FullName, destination),
                true);

            Trace.TraceInformation("Done setting configurations in the file <{0}>.", paths.Neo4JLoggingConfigFile);
        }

        internal void SetServerDbPathConfiguration(DirectoryInfo neo4JdbPath)
        {
            Trace.TraceInformation("Setting Neo4j server database path setting.");
            var patternToFind = string.Format("{0}=", paths.Neo4jDBPath);
            var dbPath = neo4JdbPath.FullName.Replace("\\", "/");

            if (paths.Neo4jDBPath == null)
            {
                //Never mount Neo4j in root of Cloud Drive. Always in Subfolder. Otherwise the Neo4j logger will crash when trying to access hidden/protected folders e.g. RecycleBin
                var dataFolder = configuration.CloudDriveDatabaseMountPath();
                dbPath += dataFolder;
            }

            var replaceWith = string.Concat(patternToFind, dbPath);
            var replacement = Replacement.Create(patternToFind, replaceWith);

            var fileName = paths.Neo4JServerConfigFile;
            fileManipulation.ReplaceConfigLine(fileName, replacement);

            var destination = Path.GetFileName(paths.Neo4JServerConfigFile.Name);
            if (destination == null)
                throw new ApplicationException("Neo4jServerConfigFile must be present.");

            File.Copy(paths.Neo4JServerConfigFile.FullName,
                      Path.Combine(paths.Neo4JLogPath.FullName, destination),
                      true);

            Trace.TraceInformation("Finished setting Neo4j server settings.");
        }
    }
}
