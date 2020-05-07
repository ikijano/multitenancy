using System.Collections.Generic;
using System.Threading.Tasks;
using Owin.MultiTenancy;

namespace Dime.Owin.MultiTenancy.Tests
{
    public class TenantResolver : ITenantResolver<Tenant>
    {
        public Task<TenantContext<Tenant>> ResolveAsync(IDictionary<string, object> environment)
        {
            environment.TryGetValue("uri", out object uri);
            Tenant tenant = uri?.ToString() switch
            {
                "https://tenant1.dimescheduler.com" => new Tenant { Name = "Contoso Inc." },
                "https://tenant2.dimescheduler.com" => new Tenant { Name = "Globex Corporation" },
                _ => null
            };

            return Task.FromResult(new TenantContext<Tenant>(tenant));
        }
    }
}