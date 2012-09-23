using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using Neo4j.Server.AzureWorkerHost;
using Xunit;

namespace Tests.NeoServerTests
{
    public class InterrogateJavaArtifactTests
    {
        [Fact]
        public void ShouldSetJavaHomePathInContext()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\temp\neo4j\jre7\jre7\bin\java.exe", new MockFileData("") }
            });
            var server = new NeoServer(new NeoServerConfiguration(), null, null, fileSystem, null);
            server.Context.JavaDirectoryPath = @"c:\temp\neo4j\jre7\";

            // Act
            server.InterrogateJavaArtifact();

            // Assert
            Assert.Equal(@"c:\temp\neo4j\jre7\jre7\", server.Context.JavaHomePath);
        }

        [Fact]
        public void ShouldThrowExceptionIfJavaExeDoesNotExist()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            var server = new NeoServer(new NeoServerConfiguration(), null, null, fileSystem, null);
            server.Context.JavaDirectoryPath = @"c:\temp\neo4j\jre7\";

            // Assert
            var ex = Assert.Throws<ApplicationException>(()
                // Act
                => server.InterrogateJavaArtifact());
            Assert.Equal(@"After downloading and unzipping the Java blob, we expected but failed to find java.exe at jre7\bin\java.exe.

On disk, this path corresponds to: c:\temp\neo4j\jre7\jre7\bin\java.exe", ex.Message);
        }
    }
}
