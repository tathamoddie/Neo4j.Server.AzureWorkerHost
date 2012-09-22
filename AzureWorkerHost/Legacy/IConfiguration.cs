namespace AzureWorkerHost.Legacy
{
    public interface IConfiguration
    {
        object Neo4JdbDriveOverridePath();
        string Neo4JdbDriveBlobSizeIfNotExists();
        string NewRelicBlobRelativePath();
        string Neo4JdbVirtualHardDriveBlobName();
        string NewRelicConfigFileName();
        string Neo4JLogsContainerName();
        string Neo4JAdminUri();
        string Neo4JDataUri();
        string Neo4JWrapperSettingLogFile();
        string Neo4JWrapperConfigFileName();
        string Neo4JConfigFileName();
        string Neo4JLoggingFileName();
        string JavaExeFileName();
        string Neo4JExeFileName();
        string Neo4JLogFolderPath();
        string Neo4JServerConfigFileName();
        string JavaBlobNameSetting();
        string CloudDriveDatabaseMountPath();
        bool Neo4JAllowStoreUpgrade();
        string Neo4JLoggingMinHeapPattern();
        string Neo4JLoggingMaxHeapPattern();
        string Neo4JLoggingMinHeap();
        string Neo4JLoggingMaxHeap();
        bool Neo4JWrapperEnableGarbageCollectionLogging();
        string Neo4JWrapperGarbageCollectionLoggingCommand();
        string JvmSwitches();
        string Neo4JLoggingLevelPattern();
        string Neo4JLoggingLevel();
        string Neo4JBlobNameSetting();
    }
}