using System;
using AzureWorkerHost.AzureMocks;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace AzureWorkerHost
{
    public class NeoServer
    {
        readonly NeoServerConfiguration configuration;
        readonly IRoleEnvironment roleEnvironment;

        internal readonly NeoRuntimeContext Context = new NeoRuntimeContext();

        public NeoServer(
            NeoServerConfiguration configuration,
            IRoleEnvironment roleEnvironment)
        {
            this.configuration = configuration;
            this.roleEnvironment = roleEnvironment;
        }

        public NeoServer(NeoServerConfiguration configuration)
            : this(configuration,
                new RoleEnvironmentWrapper())
        {}

        public void DownloadAndInstall()
        {
            InitializeLocalResource();
        }

        internal void InitializeLocalResource()
        {
            ILocalResource localResource;
            try
            {
                localResource = roleEnvironment.GetLocalResource(configuration.NeoLocalResourceName);
            }
            catch (RoleEnvironmentException ex)
            {
                throw new ApplicationException(
                    string.Format(ExceptionMessages.NeoLocalResourceNotFound, configuration.NeoLocalResourceName),
                    ex);
            }
            Context.LocalResourcePath = localResource.RootPath;
        }
    }
}
