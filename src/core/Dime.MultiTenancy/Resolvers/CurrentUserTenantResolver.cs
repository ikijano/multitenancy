using Microsoft.Owin;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Dime.MultiTenancy
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TTenant"></typeparam>
    public abstract class CurrentUserTenantResolver<TTenant> : ITenantResolver<TTenant>
    {
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
            Ensure.Argument.NotNull(environment, "environment");

            OwinContext owinContext = new OwinContext(environment);
            return this.ResolveAsync(owinContext.Request.User);
        }
    }
}