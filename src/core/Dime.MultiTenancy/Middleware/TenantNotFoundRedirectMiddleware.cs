using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dime.MultiTenancy
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TTenant"></typeparam>
    public class TenantNotFoundRedirectMiddleware<TTenant>
    {
        #region Constructor

        /// <summary>
        ///
        /// </summary>
        /// <param name="next"></param>
        /// <param name="redirectLocation"></param>
        /// <param name="permanentRedirect"></param>
        public TenantNotFoundRedirectMiddleware(
            Func<IDictionary<string, object>, Task> next,
            string redirectLocation,
            bool permanentRedirect)
        {
            Ensure.Argument.NotNull(next, "next");
            Ensure.Argument.NotNull(redirectLocation, "redirectLocation");

            this._next = next;
            this._redirectLocation = redirectLocation;
            this._permanentRedirect = permanentRedirect;
        }

        #endregion Constructor

        #region Properties

        private readonly Func<IDictionary<string, object>, Task> _next;
        private readonly string _redirectLocation;
        private readonly bool _permanentRedirect;

        #endregion Properties

        #region Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="environment"></param>
        /// <returns></returns>
        public async Task Invoke(IDictionary<string, object> environment)
        {
            Ensure.Argument.NotNull(environment, "environment");

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

            // otherwise continue processing
            await _next(environment);
        }

        #endregion Methods
    }
}