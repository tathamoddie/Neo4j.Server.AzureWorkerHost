using System;

namespace Neo4j.Server.AzureWorkerHost.Legacy
{
    [Obsolete]
    public class Replacement
    {
        private Replacement(string patternToFind, string lineToInsert,
                            LineSearchOptions lineSearchOptions = LineSearchOptions.Contains)
        {
            PatternToFind = patternToFind;
            LineToInsert = lineToInsert;
            LineSearchOptions = lineSearchOptions;
        }

        public string PatternToFind { get; set; }
        public string LineToInsert { get; set; }
        public LineSearchOptions LineSearchOptions { get; set; }

        public static Replacement Create(string patternToFind, string lineToInsert,
                                         LineSearchOptions lineSearchOptions = LineSearchOptions.Contains)
        {
            return new Replacement(patternToFind, lineToInsert, lineSearchOptions);
        }
    }
}