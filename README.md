Neo4j.Server.AzureWorkerHost
============================

Making it easy to run Neo4j as a PaaS solution in Azure.

## Project Status

Totally and utterly pre-alpha. Barely even that. A scratchpad for ideas.

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
