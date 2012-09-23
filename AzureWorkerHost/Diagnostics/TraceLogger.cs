using System.Diagnostics;

namespace Neo4j.Server.AzureWorkerHost.Diagnostics
{
    public class TraceLogger : ILogger
    {
        public TraceLogger()
        {
            Category = "NeoServer";
        }

        public string Category { get; set; }

        public void Fail(string message)
        {
            Trace.TraceError(message);
        }

        public void WriteLine(string message)
        {
            Trace.WriteLine(message);
        }
    }
}
