using System;

namespace Owin.MultiTenancy
{
    /// <summary>
    ///
    /// </summary>
    public static class AppBuilderExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TTenant"></typeparam>
        /// <param name="app"></param>
        /// <param name="tenantResolver"></param>
        /// <returns></returns>
        public static IAppBuilder UseMultiTenancy<TTenant>(this IAppBuilder app, ITenantResolver<TTenant> tenantResolver)
        {
            Ensure.Argument.NotNull(app, nameof(app));
            Ensure.Argument.NotNull(tenantResolver, nameof(tenantResolver));

            return app.UseMultiTenancy(() => tenantResolver);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TTenant"></typeparam>
        /// <param name="app"></param>
        /// <param name="tenantResolverFactory"></param>
        /// <returns></returns>
        public static IAppBuilder UseMultiTenancy<TTenant>(this IAppBuilder app, Func<ITenantResolver<TTenant>> tenantResolverFactory)
        {
            Ensure.Argument.NotNull(app, nameof(app));
            Ensure.Argument.NotNull(tenantResolverFactory, nameof(tenantResolverFactory));

            app.Use(typeof(TenantResolutionMiddleware<TTenant>), tenantResolverFactory);
            return app;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TTenant"></typeparam>
        /// <param name="app"></param>
        /// <param name="redirectLocation"></param>
        /// <param name="permanentRedirect"></param>
        /// <returns></returns>
        public static IAppBuilder RedirectIfTenantNotFound<TTenant>(this IAppBuilder app, string redirectLocation, bool permanentRedirect = false)
        {
            Ensure.Argument.NotNull(app, nameof(app));
            Ensure.Argument.NotNullOrEmpty(redirectLocation, nameof(redirectLocation));

            app.Use(typeof(TenantNotFoundRedirectMiddleware<TTenant>), redirectLocation, permanentRedirect);
            return app;
        }
    }
}