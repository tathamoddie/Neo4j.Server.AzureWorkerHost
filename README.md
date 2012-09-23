Neo4j.Server.AzureWorkerHost
============================

Making it easy to run Neo4j as a PaaS solution in Azure.

## Project Status

Just past alpha. Some known deficiencies still. Not quite beta.

There's [a super-quick screencast of what works so far](https://vimeo.com/50002373) that you should watch.

The work item backlog is on [our public Trello board](https://trello.com/b/b27rGYoY).

Builds can be tracked on [our public build server](https://tc.readifycloud.com/viewType.html?buildTypeId=bt11&guest=1).

Up-to-the-minute packages can be installed from [NuGet](https://nuget.org/packages/Neo4j.Server.AzureWorkerHost). (Every passing CI build automatically pushes straight to NuGet.)

## The Goal

1. Create an Azure solution
2. Add a worker role
3. Run `Install-Package Neo4j.Server.AzureWorkerHost` in that role
4. Hit Ctrl+F5
5. Bam! Neo4j is running locally in your Azure emulator
6. Deploy to real Azure
7. Bam! Neo4j is running in real Azure

## What Works Today

1. Create an Azure solution
1. Add a worker role
1. Run `Install-Package Neo4j.Server.AzureWorkerHost` in that role
1. Add this to your `.cscfg`:

        <LocalResources>
          <LocalStorage name="Neo4jInstall" cleanOnRoleRecycle="true" sizeInMB="500" />
        </LocalResources>
        <Endpoints>
          <InternalEndpoint name="Neo4j" protocol="tcp" />
        </Endpoints>

1. Hit Ctrl+F5
1. Upload the two blobs it tells you to
1. Hit Ctrl+F5 again
1. Bam! Neo4j is running locally in your Azure emulator
1. Deploy to real Azure
1. Bam! Neo4j is running in real Azure

## Related Works

Also from Readify:

* http://hg.readify.net/neo4jclient
* http://blog.tatham.oddie.com.au/2012/06/18/new-talks-neo4j-in-a-net-world-and-youre-in-production-now-what/
