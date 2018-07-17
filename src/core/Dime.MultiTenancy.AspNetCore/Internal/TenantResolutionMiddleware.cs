using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Dime.Multitenancy.Internal
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TTenant"></typeparam>
	public class TenantResolutionMiddleware<TTenant>
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _log;

        /// <summary>
        ///
        /// </summary>
        /// <param name="next"></param>
        /// <param name="loggerFactory"></param>
        public TenantResolutionMiddleware(
            RequestDelegate next,
            ILoggerFactory loggerFactory)
        {
            Ensure.Argument.NotNull(next, nameof(next));
            Ensure.Argument.NotNull(loggerFactory, nameof(loggerFactory));

            this._next = next;
            this._log = loggerFactory.CreateLogger<TenantResolutionMiddleware<TTenant>>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tenantResolver"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context, ITenantResolver<TTenant> tenantResolver)
        {
            Ensure.Argument.NotNull(context, nameof(context));
            Ensure.Argument.NotNull(tenantResolver, nameof(tenantResolver));

            _log.LogDebug("Resolving TenantContext using {loggerType}.", tenantResolver.GetType().Name);

            TenantContext<TTenant> tenantContext = await tenantResolver.ResolveAsync(context);

            if (tenantContext != null)
            {
                _log.LogDebug("TenantContext Resolved. Adding to HttpContext.");
                context.SetTenantContext(tenantContext);
            }
            else
                _log.LogDebug("TenantContext Not Resolved.");

            await _next.Invoke(context);
        }
    }
}