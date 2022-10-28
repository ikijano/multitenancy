using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Throw;

namespace Owin.MultiTenancy
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TTenant"></typeparam>
    public class TenantResolutionMiddleware<TTenant>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TenantResolutionMiddleware{TTenant}"/> class
        /// </summary>
        /// <param name="next"></param>
        /// <param name="tenantResolverFactory"></param>
        public TenantResolutionMiddleware(Func<IDictionary<string, object>, Task> next, Func<ITenantResolver<TTenant>> tenantResolverFactory)
        {
            next.ThrowIfNull();
            tenantResolverFactory.ThrowIfNull();

            _next = next;
            _tenantResolverFactory = tenantResolverFactory;
        }

        private readonly Func<IDictionary<string, object>, Task> _next;
        private readonly Func<ITenantResolver<TTenant>> _tenantResolverFactory;

        /// <summary>
        ///
        /// </summary>
        /// <param name="environment"></param>
        /// <returns></returns>
        public async Task Invoke(IDictionary<string, object> environment)
        {
            environment.ThrowIfNull();

            ITenantResolver<TTenant> tenantResolver = _tenantResolverFactory();
            TenantContext<TTenant> tenantContext = await tenantResolver.ResolveAsync(environment);

            if (tenantContext != null)
                environment.SetTenantContext(tenantContext);

            await _next(environment);
        }
    }
}