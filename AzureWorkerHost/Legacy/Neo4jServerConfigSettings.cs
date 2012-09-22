using System;

namespace Neo4j.Server.AzureWorkerHost.Legacy
{
    internal class Neo4JServerConfigSettings : INeo4JServerConfigSettings
    {
        public string Port
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string IpAddress
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string WebAdminDataUri
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string WebAdminManagementUri
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string DatabaseLocation
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}