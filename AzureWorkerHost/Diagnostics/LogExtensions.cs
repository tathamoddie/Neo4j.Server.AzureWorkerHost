using System;
using System.Collections.Generic;

namespace Neo4j.Server.AzureWorkerHost.Diagnostics
{
    internal static class LogExtensions
    {
        public static void Fail(this IEnumerable<ILogger> loggers, Exception exception, string message, params object[] arguments)
        {
            message = string.Format(message, arguments);
            message = message + "\r\n\r\n" + exception;
            foreach (var logger in loggers)
                logger.WriteLine(message);
        }

        public static void WriteLine(this IEnumerable<ILogger> loggers, string message, params object[] arguments)
        {
            message = string.Format(message, arguments);
            foreach (var logger in loggers)
                logger.WriteLine(message);
        }
    }
}
