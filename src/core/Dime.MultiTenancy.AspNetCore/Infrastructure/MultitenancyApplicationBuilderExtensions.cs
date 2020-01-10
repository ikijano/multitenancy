using Dime.MultiTenancy;
using Dime.MultiTenancy.Internal;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 
    /// </summary>
    public static class MultiTenancyApplicationBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TTenant"></typeparam>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseMultiTenancy<TTenant>(this IApplicationBuilder app)
        {
            Ensure.Argument.NotNull(app, nameof(app));
            return app.UseMiddleware<TenantResolutionMiddleware<TTenant>>();
        }
    }
}