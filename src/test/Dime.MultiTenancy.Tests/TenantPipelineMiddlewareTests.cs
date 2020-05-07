using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Owin.MultiTenancy;

namespace Dime.Owin.MultiTenancy.Tests
{
    [TestClass]
    public class TenantPipelineMiddlewareTests
    {
        [DataTestMethod]
        [DataRow("tenant1", "Contoso Inc.")]
        [DataRow("tenant2", "Globex Corporation")]
        public async Task TestResolutionMiddleware_WithUriResolver_ShouldResolveTenants(string subdomain, string name)
        {
            // Arrange
            IOwinRequest request = Mock.Of<IOwinRequest>();
            Mock<IOwinRequest> requestMock = Mock.Get(request);

            requestMock.Setup(_ => _.Uri).Returns(new Uri($"https://{subdomain}.dimescheduler.com"));
            requestMock.Setup(_ => _.User.Identity.IsAuthenticated).Returns(true);
            requestMock.Setup(_ => _.Headers.Append(It.IsAny<string>(), It.IsAny<string>()));

            OwinResponse response = new OwinResponse();

            ConcurrentDictionary<string, object> owinEnvironment = new ConcurrentDictionary<string, object>();
            owinEnvironment.TryAdd("uri", $"https://{subdomain}.dimescheduler.com");
            OwinContext context = Mock.Of<OwinContext>();
            Mock<OwinContext> contextMock = Mock.Get(context);
            contextMock.CallBase = true;
            contextMock.Setup(_ => _.Environment).Returns(owinEnvironment);
            contextMock.Setup(_ => _.Request).Returns(request);
            contextMock.Setup(_ => _.Response).Returns(response);

            Func<IDictionary<string, object>, Task> next = _ => Task.FromResult((object)null);
            TenantResolutionMiddleware<Tenant> multiTenancyMiddleware = new TenantResolutionMiddleware<Tenant>(next, () => new TenantResolver());

            // Act
            await multiTenancyMiddleware.Invoke(context.Environment);
            TenantContext<Tenant> tenantContext = context.Environment.GetTenantContext<Tenant>();

            // Assert
            Assert.IsTrue(tenantContext.Tenant.Name == name);
        }

        [TestMethod]
        public async Task TestResolutionMiddleware_WithUriResolver_TenantDoesNotExist_ShouldThrowArgumentNullException()
        {
            // Arrange
            IOwinRequest request = Mock.Of<IOwinRequest>();
            Mock<IOwinRequest> requestMock = Mock.Get(request);

            requestMock.Setup(_ => _.Uri).Returns(new Uri($"https://tenant3.dimescheduler.com"));
            requestMock.Setup(_ => _.User.Identity.IsAuthenticated).Returns(true);
            requestMock.Setup(_ => _.Headers.Append(It.IsAny<string>(), It.IsAny<string>()));

            OwinResponse response = new OwinResponse();

            ConcurrentDictionary<string, object> owinEnvironment = new ConcurrentDictionary<string, object>();
            owinEnvironment.TryAdd("uri", $"https://tenant3.dimescheduler.com");
            OwinContext context = Mock.Of<OwinContext>();
            Mock<OwinContext> contextMock = Mock.Get(context);
            contextMock.CallBase = true;
            contextMock.Setup(_ => _.Environment).Returns(owinEnvironment);
            contextMock.Setup(_ => _.Request).Returns(request);
            contextMock.Setup(_ => _.Response).Returns(response);

            Func<IDictionary<string, object>, Task> next = _ => Task.FromResult((object)null);
            TenantResolutionMiddleware<Tenant> multiTenancyMiddleware = new TenantResolutionMiddleware<Tenant>(next, () => new TenantResolver());

            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await multiTenancyMiddleware.Invoke(context.Environment));
        }
    }
}