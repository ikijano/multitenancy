using StructureMap;

namespace Dime.Multitenancy.StructureMap.Internal
{
    internal static class StructureMapTenantContextExtensions
    {
        private const string TenantContainerKey = "Dime.TenantContainer";

        public static IContainer GetTenantContainer<TTenant>(this TenantContext<TTenant> tenantContext)
        {
            Ensure.Argument.NotNull(tenantContext, nameof(tenantContext));

            if (tenantContext.Properties.TryGetValue(TenantContainerKey, out var tenantContainer))
            {
                return tenantContainer as IContainer;
            }

            return null;
        }

        public static void SetTenantContainer<TTenant>(this TenantContext<TTenant> tenantContext, IContainer tenantContainer)
        {
            Ensure.Argument.NotNull(tenantContext, nameof(tenantContext));
            Ensure.Argument.NotNull(tenantContainer, nameof(tenantContainer));

            tenantContext.Properties[TenantContainerKey] = tenantContainer;
        }
    }
}
