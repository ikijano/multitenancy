using System;

using Throw;

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
            app.ThrowIfNull();
            tenantResolver.ThrowIfNull();

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
            app.ThrowIfNull();
            tenantResolverFactory.ThrowIfNull();

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
            app.ThrowIfNull();
            redirectLocation.ThrowIfNull();

            app.Use(typeof(TenantNotFoundRedirectMiddleware<TTenant>), redirectLocation, permanentRedirect);
            return app;
        }
    }
}