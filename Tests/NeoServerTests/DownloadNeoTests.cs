using System;
using System.Linq;
using System.Reflection;
using AzureWorkerHost;
using Microsoft.WindowsAzure.ServiceRuntime;
using NSubstitute;
using Xunit;

namespace Tests.NeoServerTests
{
    public class DownloadNeoTests
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
                roleEnvironment);

            // Assert
            var exposedException = Assert.Throws<ApplicationException>(
                // Act
                () => server.DownloadNeo());
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
                roleEnvironment);

            // Assert
            var exposedException = Assert.Throws<ApplicationException>(
                // Act
                () => server.DownloadNeo());
            Assert.Same(thrownException, exposedException.InnerException);
        }
    }
}
