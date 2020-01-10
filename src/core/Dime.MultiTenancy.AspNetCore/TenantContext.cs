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
        private bool _disposed;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenant"></param>
        public TenantContext(TTenant tenant)
        {
            Ensure.Argument.NotNull(tenant, nameof(Tenant));

            Tenant = tenant;
            Properties = new Dictionary<string, object>();
        }

        /// <summary>
        /// 
        /// </summary>
        public string Id { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// 
        /// </summary>
        public TTenant Tenant { get; }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<string, object> Properties { get; }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                foreach (var prop in Properties)
                    TryDisposeProperty(prop.Value as IDisposable);

                TryDisposeProperty(Tenant as IDisposable);
            }

            _disposed = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void TryDisposeProperty(IDisposable obj)
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