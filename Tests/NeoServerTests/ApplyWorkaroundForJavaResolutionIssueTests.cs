using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using Neo4j.Server.AzureWorkerHost;
using Xunit;

namespace Tests.NeoServerTests
{
    public class ApplyWorkaroundForJavaResolutionIssueTests
    {
        [Fact]
        public void ShouldSilentlyExitIfBaseBatDoesNotExist()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\temp\neo4j\neo-v1234\bin\neo4j.bat", new MockFileData("foo") }
            });
            var server = new NeoServer(new NeoServerConfiguration(), null, null, fileSystem, null);
            server.Context.NeoBasePath = @"c:\temp\neo4j\neo-v1234\";

            // Act
            server.ApplyWorkaroundForJavaResolutionIssue();

            // Assert
            // Nothing crashed
        }

        [Fact]
        public void ShouldPatchLinesInBaseBatThatCallJavaDirectly()
        {
            // Arrange
            const string badFileContent = @"
foo
java bar

baz qak java qoo
ajava";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\temp\neo4j\neo-v1234\bin\neo4j.bat", new MockFileData("foo") },
                { @"c:\temp\neo4j\neo-v1234\bin\base.bat", new MockFileData(badFileContent) }
            });
            var server = new NeoServer(new NeoServerConfiguration(), null, null, fileSystem, null);
            server.Context.NeoBasePath = @"c:\temp\neo4j\neo-v1234\";

            // Act
            server.ApplyWorkaroundForJavaResolutionIssue();

            // Assert
            const string goodFileContent = @"
foo
""%javaPath%\bin\java.exe"" bar

baz qak java qoo
ajava";
            var content = fileSystem.File.ReadAllText(@"c:\temp\neo4j\neo-v1234\bin\base.bat");
            Assert.Equal(goodFileContent, content);
        }
    }
}
