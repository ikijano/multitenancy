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

            this.next = next;
            this.redirectLocation = redirectLocation;
            this.permanentRedirect = permanentRedirect;
        }

        #endregion Constructor

        #region Properties

        private readonly Func<IDictionary<string, object>, Task> next;
        private readonly string redirectLocation;
        private readonly bool permanentRedirect;

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
                owinContext.Response.Redirect(redirectLocation);

                if (permanentRedirect)
                    owinContext.Response.StatusCode = 301;

                return;
            }

            // otherwise continue processing
            await next(environment);
        }

        #endregion Methods
    }
}