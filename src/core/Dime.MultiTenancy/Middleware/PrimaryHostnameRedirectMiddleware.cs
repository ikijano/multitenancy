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
    public class PrimaryHostnameRedirectMiddleware<TTenant>
    {
        #region Constructor

        /// <summary>
        ///
        /// </summary>
        /// <param name="next"></param>
        /// <param name="primaryHostnameAccessor"></param>
        /// <param name="permanentRedirect"></param>
        public PrimaryHostnameRedirectMiddleware(
            Func<IDictionary<string, object>, Task> next,
            Func<TTenant, string> primaryHostnameAccessor,
            bool permanentRedirect)
        {
            Ensure.Argument.NotNull(next, "next");
            Ensure.Argument.NotNull(primaryHostnameAccessor, "primaryHostnameAccessor");

            this._next = next;
            this._primaryHostnameAccessor = primaryHostnameAccessor;
            this._permanentRedirect = permanentRedirect;
        }

        #endregion Constructor

        #region Properties

        private readonly Func<IDictionary<string, object>, Task> _next;
        private readonly Func<TTenant, string> _primaryHostnameAccessor;
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
            if (tenantContext != null)
            {
                string primaryHostname = _primaryHostnameAccessor(tenantContext.Tenant);
                if (!string.IsNullOrEmpty(primaryHostname))
                {
                    OwinContext owinContext = new OwinContext(environment);
                    if (!owinContext.Request.Uri.Host.Equals(primaryHostname, StringComparison.OrdinalIgnoreCase))
                    {
                        this.Redirect(owinContext, primaryHostname);
                        return;
                    }
                }
            }

            // otherwise continue processing
            await _next(environment);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="owinContext"></param>
        /// <param name="primaryHostname"></param>
        private void Redirect(IOwinContext owinContext, string primaryHostname)
        {
            UriBuilder builder = new UriBuilder(owinContext.Request.Uri) { Host = primaryHostname };

            owinContext.Response.Redirect(builder.Uri.AbsoluteUri);

            if (_permanentRedirect)
                owinContext.Response.StatusCode = 301;
        }

        #endregion Methods
    }
}