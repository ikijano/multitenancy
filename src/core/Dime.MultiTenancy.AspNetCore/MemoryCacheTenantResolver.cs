using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dime.MultiTenancy
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TTenant"></typeparam>
    public abstract class MemoryCacheTenantResolver<TTenant> : ITenantResolver<TTenant>
    {
        protected readonly IMemoryCache Cache;
        protected readonly ILogger Log;
        protected readonly MemoryCacheTenantResolverOptions Options;

        /// <summary>
        ///
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="loggerFactory"></param>
        protected MemoryCacheTenantResolver(IMemoryCache cache, ILoggerFactory loggerFactory)
            : this(cache, loggerFactory, new MemoryCacheTenantResolverOptions())
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="options"></param>
        protected MemoryCacheTenantResolver(IMemoryCache cache, ILoggerFactory loggerFactory, MemoryCacheTenantResolverOptions options)
        {
            Ensure.Argument.NotNull(cache, nameof(cache));
            Ensure.Argument.NotNull(loggerFactory, nameof(loggerFactory));
            Ensure.Argument.NotNull(options, nameof(options));

            this.Cache = cache;
            this.Log = loggerFactory.CreateLogger<MemoryCacheTenantResolver<TTenant>>();
            this.Options = options;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual MemoryCacheEntryOptions CreateCacheEntryOptions()
            => new MemoryCacheEntryOptions().SetSlidingExpiration(new TimeSpan(1, 0, 0));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="tenantContext"></param>
        protected virtual void DisposeTenantContext(object cacheKey, TenantContext<TTenant> tenantContext)
        {
            if (tenantContext == null)
                return;

            Log.LogDebug("Disposing TenantContext:{id} instance with key \"{cacheKey}\".", tenantContext.Id, cacheKey);
            tenantContext.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected abstract string GetContextIdentifier(HttpContext context);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected abstract IEnumerable<string> GetTenantIdentifiers(TenantContext<TTenant> context);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected abstract Task<TenantContext<TTenant>> ResolveAsync(HttpContext context);

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        async Task<TenantContext<TTenant>> ITenantResolver<TTenant>.ResolveAsync(HttpContext context)
        {
            Ensure.Argument.NotNull(context, nameof(context));

            // Obtain the key used to identify cached tenants from the current request
            string cacheKey = GetContextIdentifier(context);

            if (cacheKey == null)
                return null;

            if (!(Cache.Get(cacheKey) is TenantContext<TTenant> tenantContext))
            {
                Log.LogDebug("TenantContext not present in cache with key \"{cacheKey}\". Attempting to resolve.", cacheKey);
                tenantContext = await ResolveAsync(context);

                if (tenantContext != null)
                {
                    IEnumerable<string> tenantIdentifiers = GetTenantIdentifiers(tenantContext);
                    if (tenantIdentifiers == null)
                        return tenantContext;

                    MemoryCacheEntryOptions cacheEntryOptions = GetCacheEntryOptions();

                    Log.LogDebug("TenantContext:{id} resolved. Caching with keys \"{tenantIdentifiers}\".", tenantContext.Id, tenantIdentifiers);

                    foreach (var identifier in tenantIdentifiers)
                        Cache.Set(identifier, tenantContext, cacheEntryOptions);
                }
            }
            else
            {
                Log.LogDebug("TenantContext:{id} retrieved from cache with key \"{cacheKey}\".", tenantContext.Id, cacheKey);
            }

            return tenantContext;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private MemoryCacheEntryOptions GetCacheEntryOptions()
        {
            var cacheEntryOptions = CreateCacheEntryOptions();

            if (Options.EvictAllEntriesOnExpiry)
            {
                var tokenSource = new CancellationTokenSource();

                cacheEntryOptions
                    .RegisterPostEvictionCallback(
                        (key, value, reason, state) =>
                        {
                            tokenSource.Cancel();
                        })
                    .AddExpirationToken(new CancellationChangeToken(tokenSource.Token));
            }

            if (Options.DisposeOnEviction)
            {
                cacheEntryOptions
                    .RegisterPostEvictionCallback(
                        (key, value, reason, state) =>
                        {
                            DisposeTenantContext(key, value as TenantContext<TTenant>);
                        });
            }

            return cacheEntryOptions;
        }
    }
}