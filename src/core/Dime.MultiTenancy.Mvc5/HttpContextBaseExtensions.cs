﻿using Owin.MultiTenancy;

using Throw;

namespace System.Web
{
    /// <summary>
    /// Multi tenancy extensions for the <see cref="HttpContextBase"/> class.
    /// </summary>
    public static class HttpContextBaseDimeExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TTenant"></typeparam>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static TenantContext<TTenant> GetTenantContext<TTenant>(this HttpContextBase httpContext)
        {
            httpContext.ThrowIfNull();
            return httpContext.GetOwinContext().Environment.GetTenantContext<TTenant>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TTenant"></typeparam>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static TTenant GetTenant<TTenant>(this HttpContextBase httpContext)
        {
            httpContext.ThrowIfNull();
            return httpContext.GetOwinContext().Environment.GetTenant<TTenant>();
        }
    }
}