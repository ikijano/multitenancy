using Dime.Multitenancy;
using Dime.Multitenancy.Internal;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 
    /// </summary>
    public static class MultitenancyApplicationBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TTenant"></typeparam>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseMultitenancy<TTenant>(this IApplicationBuilder app)
        {
            Ensure.Argument.NotNull(app, nameof(app));
            return app.UseMiddleware<TenantResolutionMiddleware<TTenant>>();
        }
    }
}