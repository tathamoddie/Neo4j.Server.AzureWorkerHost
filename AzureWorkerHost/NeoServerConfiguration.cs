namespace Neo4j.Server.AzureWorkerHost
{
    public class NeoServerConfiguration
    {
        public string NeoLocalResourceName = "Neo4jInstall";
        public string JavaBlobName = "neo4j/jre7.zip";
        public string JavaDirectoryName = "jre7";
        public string JavaExeRelativePath = @"jre7\bin\java.exe";
        public string NeoBlobName = "neo4j/neo4j-community-1.8.RC1-windows.zip";
        public string NeoDirectoryName = "neo4j-inst";
        public string NeoEndpointId = "Neo4j";
        public string NeoBatRelativePath = @"neo4j-community-1.8.RC1\bin\neo4j.bat";
    }
}
