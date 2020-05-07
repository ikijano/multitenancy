using System.Collections.Generic;
using System.Threading.Tasks;

namespace Owin.MultiTenancy
{
    /// <summary>
    /// Represents a class that is able to map the OWIN request context to a tenant
    /// </summary>
    /// <typeparam name="TTenant">The tenant type</typeparam>
    public interface ITenantResolver<TTenant>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="environment"></param>
        /// <returns></returns>
        Task<TenantContext<TTenant>> ResolveAsync(IDictionary<string, object> environment);
    }
}