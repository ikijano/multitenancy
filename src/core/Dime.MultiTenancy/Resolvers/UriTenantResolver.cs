using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Owin;

using Throw;

namespace Owin.MultiTenancy
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TTenant"></typeparam>
    public abstract class UriTenantResolver<TTenant> : ITenantResolver<TTenant>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UriTenantResolver{TTenant}"/> class
        /// </summary>
        protected UriTenantResolver()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public abstract Task<TenantContext<TTenant>> ResolveAsync(Uri uri);

        /// <summary>
        ///
        /// </summary>
        /// <param name="environment"></param>
        /// <returns></returns>
        Task<TenantContext<TTenant>> ITenantResolver<TTenant>.ResolveAsync(IDictionary<string, object> environment)
        {
            environment.ThrowIfNull();

            OwinContext owinContext = new OwinContext(environment);
            return ResolveAsync(owinContext.Request.Uri);
        }
    }
}