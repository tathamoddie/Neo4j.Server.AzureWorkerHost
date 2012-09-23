namespace Neo4j.Server.AzureWorkerHost
{
    public class NeoServerConfiguration
    {
        public string NeoLocalResourceName = "Neo4jInstall";

        /// <summary>
        /// Name of the blob that Java is in, relative to the blob root
        /// </summary>
        public string JavaBlobName = "neo4j/jre7.zip";

        /// <summary>
        /// Name of the directory to unzip the Java blob into
        /// </summary>
        public string JavaDirectoryName = "jre7";

        /// <summary>
        /// Folder path relative to <see cref="JavaDirectoryName"/> that corresponds to something usable as JAVA_HOME
        /// </summary>
        public string JavaHomeRelativePath = @"jre7\";

        public string NeoBlobName = "neo4j/neo4j-community-1.8.RC1-windows.zip";
        public string NeoDirectoryName = "neo4j-inst";
        public string NeoEndpointId = "Neo4j";

        /// <summary>
        /// Folder path relative to <see cref="NeoDirectoryName"/> that corresponds to the root of the Neo distribution (must have child folders like bin and conf)
        /// </summary>
        public string NeoBasePath = @"neo4j-community-1.8.RC1";
    }
}
