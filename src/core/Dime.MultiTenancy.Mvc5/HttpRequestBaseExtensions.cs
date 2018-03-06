using Dime.MultiTenancy;

namespace System.Web
{
    /// <summary>
    ///
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
            Ensure.Argument.NotNull(request, "request");
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
            Ensure.Argument.NotNull(request, "request");
            return request.GetOwinContext().Environment.GetTenant<TTenant>();
        }
    }
}