using System.Collections.Generic;

namespace Dime.MultiTenancy
{
    /// <summary>
    ///
    /// </summary>
    public static class OwinEnvironmentExtensions
    {
        /// <summary>
        /// The key that is used to store the tenant context in the owin environment
        /// </summary>
        private const string TenantContextKey = "dimescheduler:tenantContext";

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TTenant"></typeparam>
        /// <param name="environment"></param>
        /// <param name="tenantContext"></param>
        public static void SetTenantContext<TTenant>(this IDictionary<string, object> environment, TenantContext<TTenant> tenantContext)
        {
            Ensure.Argument.NotNull(environment, "environment");
            Ensure.Argument.NotNull(tenantContext, "tenantContext");

            environment.AddOrUpdate(TenantContextKey, tenantContext);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TTenant"></typeparam>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static TenantContext<TTenant> GetTenantContext<TTenant>(this IDictionary<string, object> environment)
        {
            Ensure.Argument.NotNull(environment, "environment");

            if (environment.TryGetValue(TenantContextKey, out var tenantContext))
                return tenantContext as TenantContext<TTenant>;

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TTenant"></typeparam>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static TTenant GetTenant<TTenant>(this IDictionary<string, object> environment)
        {
            Ensure.Argument.NotNull(environment, "environment");

            TenantContext<TTenant> tenantContext = GetTenantContext<TTenant>(environment);
            if (tenantContext != null)
                return tenantContext.Tenant;

            return default(TTenant);
        }
    }
}