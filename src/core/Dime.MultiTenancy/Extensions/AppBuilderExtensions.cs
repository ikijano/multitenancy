using Dime.MultiTenancy;
using System;

namespace Owin
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
        public static IAppBuilder UseMultitenancy<TTenant>(this IAppBuilder app, ITenantResolver<TTenant> tenantResolver)
        {
            Ensure.Argument.NotNull(app, "app");
            Ensure.Argument.NotNull(tenantResolver, "tenantResolver");

            return app.UseMultitenancy(() => tenantResolver);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TTenant"></typeparam>
        /// <param name="app"></param>
        /// <param name="tenantResolverFactory"></param>
        /// <returns></returns>
        public static IAppBuilder UseMultitenancy<TTenant>(this IAppBuilder app, Func<ITenantResolver<TTenant>> tenantResolverFactory)
        {
            Ensure.Argument.NotNull(app, "app");
            Ensure.Argument.NotNull(tenantResolverFactory, "tenantResolverFactory");

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
            Ensure.Argument.NotNull(app, "app");
            Ensure.Argument.NotNullOrEmpty(redirectLocation, "redirectLocation");

            app.Use(typeof(TenantNotFoundRedirectMiddleware<TTenant>), redirectLocation, permanentRedirect);
            return app;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TTenant"></typeparam>
        /// <param name="app"></param>
        /// <param name="primaryHostnameAccessor"></param>
        /// <param name="permanentRedirect"></param>
        /// <returns></returns>
        public static IAppBuilder RedirectToPrimaryHostname<TTenant>(this IAppBuilder app, Func<TTenant, string> primaryHostnameAccessor, bool permanentRedirect = true)
        {
            Ensure.Argument.NotNull(app, "app");
            Ensure.Argument.NotNull(primaryHostnameAccessor, "primaryHostnameAccessor");

            app.Use(typeof(PrimaryHostnameRedirectMiddleware<TTenant>),
                primaryHostnameAccessor, permanentRedirect);

            return app;
        }
    }
}