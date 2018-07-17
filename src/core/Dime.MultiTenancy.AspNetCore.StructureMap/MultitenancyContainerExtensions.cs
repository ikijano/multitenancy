using Dime.Multitenancy;
using Dime.Multitenancy.StructureMap;
using System;

namespace StructureMap
{
    /// <summary>
    /// 
    /// </summary>
    public static class MultitenancyContainerExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TTenant"></typeparam>
        /// <param name="container"></param>
        /// <param name="configure"></param>
        public static void ConfigureTenants<TTenant>(this IContainer container, Action<ConfigurationExpression> configure)
        {
            Ensure.Argument.NotNull(container, nameof(container));
            Ensure.Argument.NotNull(configure, nameof(configure));

            container.Configure(_ =>
                _.For<ITenantContainerBuilder<TTenant>>()
                    .Use(new StructureMapTenantContainerBuilder<TTenant>(container, (tenant, config) => configure(config)))
            );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TTenant"></typeparam>
        /// <param name="container"></param>
        /// <param name="configure"></param>
        public static void ConfigureTenants<TTenant>(this IContainer container, Action<TTenant, ConfigurationExpression> configure)
        {
            Ensure.Argument.NotNull(container, nameof(container));
            Ensure.Argument.NotNull(configure, nameof(configure));

            container.Configure(_ =>
                _.For<ITenantContainerBuilder<TTenant>>()
                    .Use(new StructureMapTenantContainerBuilder<TTenant>(container, configure))
            );
        }
    }
}