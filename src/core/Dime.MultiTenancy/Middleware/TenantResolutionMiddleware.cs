using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dime.MultiTenancy
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TTenant"></typeparam>
    public class TenantResolutionMiddleware<TTenant>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="next"></param>
        /// <param name="tenantResolverFactory"></param>
        public TenantResolutionMiddleware(Func<IDictionary<string, object>, Task> next, Func<ITenantResolver<TTenant>> tenantResolverFactory)
        {
            Ensure.Argument.NotNull(next, "next");
            Ensure.Argument.NotNull(tenantResolverFactory, "tenantResolverFactory");

            this._next = next;
            this._tenantResolverFactory = tenantResolverFactory;
        }

        #region Properties

        private readonly Func<IDictionary<string, object>, Task> _next;
        private readonly Func<ITenantResolver<TTenant>> _tenantResolverFactory;

        #endregion Properties

        #region Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="environment"></param>
        /// <returns></returns>
        public async Task Invoke(IDictionary<string, object> environment)
        {
            Ensure.Argument.NotNull(environment, "environment");

            ITenantResolver<TTenant> tenantResolver = _tenantResolverFactory();
            TenantContext<TTenant> tenantContext = await tenantResolver.ResolveAsync(environment);

            if (tenantContext != null)
                environment.SetTenantContext(tenantContext);

            await _next(environment);
        }

        #endregion Methods
    }
}