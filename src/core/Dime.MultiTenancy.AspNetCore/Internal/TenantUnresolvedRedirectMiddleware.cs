using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Dime.Multitenancy.Internal
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TTenant"></typeparam>
    public class TenantUnresolvedRedirectMiddleware<TTenant>
    {
        private readonly string _redirectLocation;
        private readonly bool _permanentRedirect;
        private readonly RequestDelegate _next;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="redirectLocation"></param>
        /// <param name="permanentRedirect"></param>
        public TenantUnresolvedRedirectMiddleware(
            RequestDelegate next,
            string redirectLocation,
            bool permanentRedirect)
        {
            Ensure.Argument.NotNull(next, nameof(next));
            Ensure.Argument.NotNull(redirectLocation, nameof(redirectLocation));

            this._next = next;
            this._redirectLocation = redirectLocation;
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
            if (tenantContext == null)
            {
                Redirect(context, _redirectLocation);
                return;
            }

            // otherwise continue processing
            await _next(context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="redirectLocation"></param>
        private void Redirect(HttpContext context, string redirectLocation)
        {
            context.Response.Redirect(redirectLocation);
            context.Response.StatusCode = _permanentRedirect ? StatusCodes.Status301MovedPermanently : StatusCodes.Status302Found;
        }
    }
}