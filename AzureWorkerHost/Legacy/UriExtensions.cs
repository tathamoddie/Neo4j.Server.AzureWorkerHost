using System;

namespace Neo4j.Server.AzureWorkerHost.Legacy
{
    [Obsolete]
    internal static class UriExtensions
    {
        public static Uri Append(this Uri baseUri, string relativeUriPathToAppend)
        {
            var uriBuilder = new UriBuilder(baseUri);
            var str = uriBuilder.Path.TrimEnd(new[] { '/' });
            relativeUriPathToAppend = relativeUriPathToAppend.TrimStart(new[] { '/' });
            uriBuilder.Path = str + "/" + relativeUriPathToAppend;
            return uriBuilder.Uri;
        }
    }
}