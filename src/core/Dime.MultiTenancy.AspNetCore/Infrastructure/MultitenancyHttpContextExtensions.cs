using Dime.Multitenancy;

namespace Microsoft.AspNetCore.Http
{
    /// <summary>
    /// Multitenant extensions for <see cref="HttpContext"/>.
    /// </summary>
    public static class MultitenancyHttpContextExtensions
    {
        private const string TenantContextKey = "Dime.TenantContext";

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TTenant"></typeparam>
        /// <param name="context"></param>
        /// <param name="tenantContext"></param>
        public static void SetTenantContext<TTenant>(this HttpContext context, TenantContext<TTenant> tenantContext)
        {
            Ensure.Argument.NotNull(context, nameof(context));
            Ensure.Argument.NotNull(tenantContext, nameof(tenantContext));

            context.Items[TenantContextKey] = tenantContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TTenant"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public static TenantContext<TTenant> GetTenantContext<TTenant>(this HttpContext context)
        {
            Ensure.Argument.NotNull(context, nameof(context));

            return context.Items.TryGetValue(TenantContextKey, out var tenantContext)
                ? tenantContext as TenantContext<TTenant>
                : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TTenant"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public static TTenant GetTenant<TTenant>(this HttpContext context)
        {
            Ensure.Argument.NotNull(context, nameof(context));

            TenantContext<TTenant> tenantContext = GetTenantContext<TTenant>(context);
            return tenantContext != null ? tenantContext.Tenant : default(TTenant);
        }
    }
}