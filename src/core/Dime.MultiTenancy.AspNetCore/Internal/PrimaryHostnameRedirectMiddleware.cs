using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Threading.Tasks;

namespace Dime.MultiTenancy.Internal
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TTenant"></typeparam>
    public class PrimaryHostnameRedirectMiddleware<TTenant>
    {
        private readonly Func<TTenant, string> _primaryHostnameAccessor;
        private readonly bool _permanentRedirect;
        private readonly RequestDelegate _next;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="primaryHostnameAccessor"></param>
        /// <param name="permanentRedirect"></param>
        public PrimaryHostnameRedirectMiddleware(
            RequestDelegate next,
            Func<TTenant, string> primaryHostnameAccessor,
            bool permanentRedirect)
        {
            Ensure.Argument.NotNull(next, nameof(next));
            Ensure.Argument.NotNull(primaryHostnameAccessor, nameof(primaryHostnameAccessor));

            this._next = next;
            this._primaryHostnameAccessor = primaryHostnameAccessor;
            this._permanentRedirect = permanentRedirect;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            Ensure.Argument.NotNull(context, nameof(context));

            TenantContext<TTenant> tenantContext = context.GetTenantContext<TTenant>();
            if (tenantContext != null)
            {
                string primaryHostname = _primaryHostnameAccessor(tenantContext.Tenant);
                if (!string.IsNullOrWhiteSpace(primaryHostname))
                {
                    if (!context.Request.Host.Value.Equals(primaryHostname, StringComparison.OrdinalIgnoreCase))
                    {
                        Redirect(context, primaryHostname);
                        return;
                    }
                }
            }

            // otherwise continue processing
            await _next(context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="primaryHostname"></param>
        private void Redirect(HttpContext context, string primaryHostname)
        {
            var builder = new UriBuilder(context.Request.GetEncodedUrl()) { Host = primaryHostname };

            context.Response.Redirect(builder.Uri.AbsoluteUri);
            context.Response.StatusCode = _permanentRedirect ? StatusCodes.Status301MovedPermanently : StatusCodes.Status302Found;
        }
    }
}