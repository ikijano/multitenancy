using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Owin;

using Throw;

namespace Owin.MultiTenancy
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TTenant"></typeparam>
    public abstract class CurrentUserTenantResolver<TTenant> : ITenantResolver<TTenant>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentUserTenantResolver{TTenant}"/> class
        /// </summary>
        protected CurrentUserTenantResolver()
        {

        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public abstract Task<TenantContext<TTenant>> ResolveAsync(IPrincipal user);

        /// <summary>
        ///
        /// </summary>
        /// <param name="environment"></param>
        /// <returns></returns>
        Task<TenantContext<TTenant>> ITenantResolver<TTenant>.ResolveAsync(IDictionary<string, object> environment)
        {
            environment.ThrowIfNull();

            OwinContext owinContext = new OwinContext(environment);
            return ResolveAsync(owinContext.Request.User);
        }
    }
}