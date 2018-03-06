using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dime.MultiTenancy
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TTenant"></typeparam>
    public abstract class UriTenantResolver<TTenant> : ITenantResolver<TTenant>
    {
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
            Ensure.Argument.NotNull(environment, "environment");

            OwinContext owinContext = new OwinContext(environment);
            return this.ResolveAsync(owinContext.Request.Uri);
        }
    }
}