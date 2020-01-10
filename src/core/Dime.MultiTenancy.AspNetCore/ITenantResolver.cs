using Microsoft.AspNetCore.Http;
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
        /// <param name="context"></param>
        /// <returns></returns>
        Task<TenantContext<TTenant>> ResolveAsync(HttpContext context);
    }
}