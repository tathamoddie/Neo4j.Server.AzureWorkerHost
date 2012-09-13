Neo4j.Server.AzureWorkerHost
============================

Making it easy to run Neo4j as a PaaS solution in Azure.

## Project Status

Totally and utterly pre-alpha. Barely even that. A scratchpad for ideas.

Progress can be tracked on [our public Trello board](https://trello.com/b/b27rGYoY).

Builds can be tracked on [our public build server](https://tc.readifycloud.com/viewType.html?buildTypeId=bt11&guest=1).

Up-to-the-minute packages can be installed from [NuGet](https://nuget.org/packages/Neo4j.Server.AzureWorkerHost). (Every CI build automatically pushes straight to NuGet.)

In the mean time, check out our other Neo4j related work here:

* http://hg.readify.net/neo4jclient
* http://blog.tatham.oddie.com.au/2012/06/18/new-talks-neo4j-in-a-net-world-and-youre-in-production-now-what/

## The Goal

1. Create an Azure solution
2. Add a worker role
3. Run `Install-Package Neo4j.Server.AzureWorkerHost` in that role
4. Hit Ctrl+F5
5. Bam! Neo4j is running locally in your Azure emulator
6. Deploy to real Azure
7. Bam! Neo4j is running in real Azure
