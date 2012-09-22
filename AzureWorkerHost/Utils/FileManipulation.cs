using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AzureWorkerHost.Utils
{
    public class FileManipulation : IFileManipulation
    {
        public void ReplaceConfigLine(FileInfo fileToRead, params Replacement[] replacements)
        {
            ReplaceLineOrValue(fileToRead, ReplaceType.Line ,replacements);
        }

        static void ReplaceLineOrValue(FileInfo fileToRead, ReplaceType type, Replacement[] replacements, string oldValue = null)
        {
            var fileName = fileToRead.FullName;
            Trace.TraceInformation("Reading config file '{0}'.", fileName);

            if (!File.Exists(fileName))
            {
                Trace.TraceError("The file to modify does not exis '{0}'.", fileName);
                return;
            }

            var lines = new List<string>();

            using (var file = new StreamReader(fileName))
            {
                while (!file.EndOfStream)
                {
                    var line = file.ReadLine();

                    if (line == null) continue;

                    var replacement = replacements.Where(rep => IsMatch(rep, line)).SingleOrDefault();
                    if (replacement != null)
                    {
                        Trace.TraceInformation(
                            "Found match for <{0}> on line #{1}. The line was <{2}> and was replaced with <{3}>.",
                            replacement.PatternToFind,
                            lines.Count + 1,
                            line,
                            replacement.LineToInsert);
                        if(type == ReplaceType.Line)
                        line = replacement.LineToInsert;

                        if(type == ReplaceType.Value && oldValue != null)
                            line = line.Replace(oldValue, replacement.LineToInsert);
                    }

                    lines.Add(line);
                }
            }

            using (var file = new StreamWriter(fileName, false))
            {
                foreach (string line in lines)
                {
                    file.WriteLine(line);
                }
            }

            Trace.TraceInformation("Config file '{0}' written.", fileName);
        }

        public void ReplaceTextInConfigLine(FileInfo fileToRead,string oldValue, params Replacement[] replacements)
        {
            ReplaceLineOrValue(fileToRead, ReplaceType.Value, replacements, oldValue);
        }

        public void AddTextToConfigLine(FileInfo fileToRead, string configLine)
        {
            var fileName = fileToRead.FullName;
            Trace.TraceInformation("Reading config file '{0}'.", fileName);

            if (!File.Exists(fileName))
            {
                Trace.TraceError("The file to modify does not exis '{0}'.", fileName);
                return;
            }

            using(var appender = fileToRead.AppendText())
            {
                appender.WriteLine(configLine);
            }
        }

        private static bool IsMatch(Replacement rep, string line)
        {
            return rep.LineSearchOptions == LineSearchOptions.Contains
                       ? line.Contains(rep.PatternToFind)
                       : line.StartsWith(rep.PatternToFind);
        }
    }
}