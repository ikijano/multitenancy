using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Dime.Multitenancy
{
    public interface ITenantResolver<TTenant>
    {
        Task<TenantContext<TTenant>> ResolveAsync(HttpContext context);
    }
}