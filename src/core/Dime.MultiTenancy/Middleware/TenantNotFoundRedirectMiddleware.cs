using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Owin;

using Throw;

namespace Owin.MultiTenancy
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TTenant"></typeparam>
    public class TenantNotFoundRedirectMiddleware<TTenant>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TenantNotFoundRedirectMiddleware{TTenant}"/> class
        /// </summary>
        /// <param name="next"></param>
        /// <param name="redirectLocation"></param>
        /// <param name="permanentRedirect"></param>
        public TenantNotFoundRedirectMiddleware(Func<IDictionary<string, object>, Task> next, string redirectLocation, bool permanentRedirect)
        {
            next.ThrowIfNull();
            redirectLocation.ThrowIfNull();

            _next = next;
            _redirectLocation = redirectLocation;
            _permanentRedirect = permanentRedirect;
        }

        private readonly Func<IDictionary<string, object>, Task> _next;
        private readonly string _redirectLocation;
        private readonly bool _permanentRedirect;

        /// <summary>
        ///
        /// </summary>
        /// <param name="environment"></param>
        /// <returns></returns>
        public async Task Invoke(IDictionary<string, object> environment)
        {
            environment.ThrowIfNull();

            TenantContext<TTenant> tenantContext = environment.GetTenantContext<TTenant>();
            if (tenantContext == null)
            {
                // Redirect to the specified location
                OwinContext owinContext = new OwinContext(environment);
                owinContext.Response.Redirect(_redirectLocation);

                if (_permanentRedirect)
                    owinContext.Response.StatusCode = 301;

                return;
            }

            // Otherwise continue processing
            await _next(environment);
        }
    }
}