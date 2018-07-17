using Dime.Multitenancy;
using Dime.Multitenancy.Internal;
using System;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 
    /// </summary>
    public static class UsePerTenantApplicationBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TTenant"></typeparam>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IApplicationBuilder UsePerTenant<TTenant>(this IApplicationBuilder app, Action<TenantPipelineBuilderContext<TTenant>, IApplicationBuilder> configuration)
        {
            Ensure.Argument.NotNull(app, nameof(app));
            Ensure.Argument.NotNull(configuration, nameof(configuration));

            app.Use(next => new TenantPipelineMiddleware<TTenant>(next, app, configuration).Invoke);
            return app;
        }
    }
}