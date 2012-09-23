using System.Diagnostics;
using System.Net;

namespace Neo4j.Server.AzureWorkerHost
{
    internal class NeoRuntimeContext
    {
        /// <summary>
        /// Path on disk of the Azure local resource where Neo4j and all supporting files will be installed and run from
        /// </summary>
        public string LocalResourcePath { get; set; }

        /// <summary>
        /// Path on disk where Java will be unzipped to
        /// </summary>
        public string JavaDirectoryPath { get; set; }

        /// <summary>
        /// Path on disk where Neo will be unzipped to
        /// </summary>
        public string NeoDirectoryPath { get; set; }

        /// <summary>
        /// Full path to directory on disk that can be used as JAVA_HOME (should have bin\java.exe in it)
        /// </summary>
        public string JavaHomePath { get; set; }

        /// <summary>
        /// Full path to neo4j.bat on disk
        /// </summary>
        public string NeoBatPath { get; set; }

        /// <summary>
        /// Endpoint that Neo4j has to listen on to be available to other roles
        /// </summary>
        public IPEndPoint NeoEndpoint { get; set; }

        /// <summary>
        /// Handle to the running process
        /// </summary>
        public Process NeoProcess { get; set; }
    }
}
