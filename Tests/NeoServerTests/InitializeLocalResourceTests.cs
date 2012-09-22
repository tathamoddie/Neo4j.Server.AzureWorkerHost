using System;
using System.Linq;
using System.Reflection;
using Microsoft.WindowsAzure.ServiceRuntime;
using NSubstitute;
using Neo4j.Server.AzureWorkerHost;
using Neo4j.Server.AzureWorkerHost.AzureMocks;
using Tests.AzureMocks;
using Xunit;

namespace Tests.NeoServerTests
{
    public class InitializeLocalResourceTests
    {
        [Fact]
        public void ShouldProvideCustomExceptionMessageWhenLocalResourceDoesNotExist()
        {
            // Arrange
            var thrownException = (RoleEnvironmentException)typeof(RoleEnvironmentException)
                .GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
                .Single(c => c.GetParameters().Count() == 1)
                .Invoke(new object[] { "The local resource isn't defined or doesn't exist." });
            var roleEnvironment = Substitute.For<IRoleEnvironment>();
            roleEnvironment
                .GetLocalResource("foo")
                .Returns(ci => { throw thrownException; });
            var server = new NeoServer(
                new NeoServerConfiguration { NeoLocalResourceName = "foo" },
                roleEnvironment,
                null, null, null);

            // Assert
            var exposedException = Assert.Throws<ApplicationException>(
                // Act
                () => server.InitializeLocalResource());
            var expectedMessage = string.Format(ExceptionMessages.NeoLocalResourceNotFound, "foo");
            Assert.Equal(expectedMessage, exposedException.Message);
        }

        [Fact]
        public void ShouldWrapRoleEnvironmentExceptionWhenLocalResourceDoesNotExist()
        {
            // Arrange
            var thrownException = (RoleEnvironmentException)typeof (RoleEnvironmentException)
                .GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
                .Single(c => c.GetParameters().Count() == 1)
                .Invoke(new object[] { "The local resource isn't defined or doesn't exist." });
            var roleEnvironment = Substitute.For<IRoleEnvironment>();
            roleEnvironment
                .GetLocalResource("foo")
                .Returns(ci => { throw thrownException; });
            var server = new NeoServer(
                new NeoServerConfiguration { NeoLocalResourceName = "foo"},
                roleEnvironment,
                null, null, null);

            // Assert
            var exposedException = Assert.Throws<ApplicationException>(
                // Act
                () => server.InitializeLocalResource());
            Assert.Same(thrownException, exposedException.InnerException);
        }

        [Fact]
        public void ShouldSetPathIntoContext()
        {
            // Arrange
            var roleEnvironment = Substitute.For<IRoleEnvironment>();
            roleEnvironment
                .GetLocalResource("foo")
                .Returns(new MockLocalResource { RootPath = @"t:\SomePath" });
            var server = new NeoServer(
                new NeoServerConfiguration { NeoLocalResourceName = "foo" },
                roleEnvironment,
                null, null, null);

            // Act
            server.InitializeLocalResource();

            // Asset
            Assert.Equal(@"t:\SomePath", server.Context.LocalResourcePath);
        }
    }
}
