using System;
using System.Collections.Generic;

namespace Dime.MultiTenancy
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TTenant"></typeparam>
    public class TenantContext<TTenant> : IDisposable
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="tenant"></param>
        public TenantContext(TTenant tenant)
        {
            Ensure.Argument.NotNull(tenant, "tenant");

            Tenant = tenant;
            Properties = new Dictionary<string, object>();
        }

        public TTenant Tenant { get; private set; }
        public IDictionary<string, object> Properties { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public void Dispose()
        {
            foreach (var prop in Properties)
            {
                TryDispose(prop.Value as IDisposable);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        private void TryDispose(IDisposable obj)
        {
            if (obj == null)
                return;

            try
            {
                obj.Dispose();
            }
            catch (ObjectDisposedException) { }
        }
    }
}