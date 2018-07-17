using StructureMap;
using System;
using System.Threading.Tasks;

namespace Dime.Multitenancy.StructureMap
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TTenant"></typeparam>
    public class StructureMapTenantContainerBuilder<TTenant> : ITenantContainerBuilder<TTenant>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="configure"></param>
        public StructureMapTenantContainerBuilder(IContainer container, Action<TTenant, ConfigurationExpression> configure)
        {
            Ensure.Argument.NotNull(container, nameof(container));
            Ensure.Argument.NotNull(configure, nameof(configure));

            Container = container;
            Configure = configure;
        }

        protected IContainer Container { get; }
        protected Action<TTenant, ConfigurationExpression> Configure { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenant"></param>
        /// <returns></returns>
        public virtual Task<IContainer> BuildAsync(TTenant tenant)
        {
            Ensure.Argument.NotNull(tenant, nameof(tenant));

            IContainer tenantContainer = Container.CreateChildContainer();
            tenantContainer.Configure(config => Configure(tenant, config));

            return Task.FromResult(tenantContainer);
        }
    }
}