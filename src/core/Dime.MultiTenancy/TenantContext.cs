using System;
using System.Collections.Generic;

namespace Owin.MultiTenancy
{
    /// <summary>
    /// Represents an object that holds all the information of the tenant.
    /// </summary>
    /// <typeparam name="TTenant">The tenant type</typeparam>
    public class TenantContext<TTenant> : IDisposable
    {
        /// <summary>
        ///  Initializes a new instance of the <see cref="TenantContext{TTenant}"/> class
        /// </summary>
        /// <param name="tenant"></param>
        public TenantContext(TTenant tenant)
        {
            Ensure.Argument.NotNull(tenant, nameof(tenant));

            Tenant = tenant;
            Properties = new Dictionary<string, object>();
        }

        public TTenant Tenant { get; }
        public IDictionary<string, object> Properties { get; }

        /// <summary>
        ///
        /// </summary>
        public void Dispose()
        {
            foreach (KeyValuePair<string, object> prop in Properties)
                TryDispose(prop.Value as IDisposable);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        private static void TryDispose(IDisposable obj)
        {
            if (obj == null)
                return;

            try
            {
                obj.Dispose();
            }
            catch (ObjectDisposedException)
            {
            }
        }
    }
}