using Owin;
using Owin.MultiTenancy;

namespace System.Web
{
    /// <summary>
    /// Multi tenancy extensions for the <see cref="HttpRequestBase"/> class.
    /// </summary>
    public static class HttpRequestBaseExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TTenant"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public static TenantContext<TTenant> GetTenantContext<TTenant>(this HttpRequestBase request)
        {
            Ensure.Argument.NotNull(request, nameof(request));
            return request.GetOwinContext().Environment.GetTenantContext<TTenant>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TTenant"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public static TTenant GetTenant<TTenant>(this HttpRequestBase request)
        {
            Ensure.Argument.NotNull(request, nameof(request));
            return request.GetOwinContext().Environment.GetTenant<TTenant>();
        }
    }
}