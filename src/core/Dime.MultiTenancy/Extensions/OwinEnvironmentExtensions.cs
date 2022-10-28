using System;
using System.Collections.Generic;

using Throw;

namespace Owin.MultiTenancy
{
    /// <summary>
    ///
    /// </summary>
    public static class OwinEnvironmentExtensions
    {
        /// <summary>
        /// The key that is used to store the tenant context in the owin environment
        /// </summary>
        private const string TenantContextKey = "owin:tenantContext";

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TTenant"></typeparam>
        /// <param name="environment"></param>
        /// <param name="tenantContext"></param>
        public static void SetTenantContext<TTenant>(this IDictionary<string, object> environment, TenantContext<TTenant> tenantContext)
        {
            environment.ThrowIfNull();
            tenantContext.ThrowIfNull();

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
            environment.ThrowIfNull();

            if (environment.TryGetValue(TenantContextKey, out object tenantContext))
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
            environment.ThrowIfNull();

            TenantContext<TTenant> tenantContext = GetTenantContext<TTenant>(environment);
            return tenantContext != null ? tenantContext.Tenant : default;
        }
    }
}