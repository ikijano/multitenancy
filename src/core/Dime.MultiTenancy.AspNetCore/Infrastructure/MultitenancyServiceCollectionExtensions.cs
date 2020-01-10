using Dime.MultiTenancy;
using Dime.MultiTenancy.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///
    /// </summary>
    public static class MultiTenancyServiceCollectionExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TTenant"></typeparam>
        /// <typeparam name="TResolver"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMultiTenancy<TTenant, TResolver>(this IServiceCollection services)
            where TResolver : class, ITenantResolver<TTenant>
            where TTenant : class
        {
            Ensure.Argument.NotNull(services, nameof(services));

            services.AddScoped<ITenantResolver<TTenant>, TResolver>();

            // No longer registered by default as of ASP.NET Core RC2
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Make Tenant and TenantContext injectable
            services.AddScoped(prov => prov.GetService<IHttpContextAccessor>()?.HttpContext?.GetTenantContext<TTenant>());
            services.AddScoped(prov => prov.GetService<TenantContext<TTenant>>()?.Tenant);

            // Make ITenant injectable for handling null injection, similar to IOptions
            services.AddScoped<ITenant<TTenant>>(prov => new TenantWrapper<TTenant>(prov.GetService<TTenant>()));

            // Ensure caching is available for caching resolvers
            var resolverType = typeof(TResolver);
            if (typeof(MemoryCacheTenantResolver<TTenant>).IsAssignableFrom(resolverType))
                services.AddMemoryCache();

            return services;
        }
    }
}