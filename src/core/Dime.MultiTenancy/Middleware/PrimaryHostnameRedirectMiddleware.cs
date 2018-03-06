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

            this.Next = next;
            this.PrimaryHostnameAccessor = primaryHostnameAccessor;
            this.PermanentRedirect = permanentRedirect;
        }

        #endregion Constructor

        #region Properties

        private readonly Func<IDictionary<string, object>, Task> Next;
        private readonly Func<TTenant, string> PrimaryHostnameAccessor;
        private readonly bool PermanentRedirect;

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
                string primaryHostname = PrimaryHostnameAccessor(tenantContext.Tenant);
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
            await Next(environment);
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

            if (PermanentRedirect)
                owinContext.Response.StatusCode = 301;
        }

        #endregion Methods
    }
}