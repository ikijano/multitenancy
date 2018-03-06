using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dime.MultiTenancy
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TTenant"></typeparam>
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