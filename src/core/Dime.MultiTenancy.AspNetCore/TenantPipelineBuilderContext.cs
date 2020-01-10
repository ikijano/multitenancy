namespace Dime.MultiTenancy
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TTenant"></typeparam>
    public class TenantPipelineBuilderContext<TTenant>
    {
        /// <summary>
        /// 
        /// </summary>
        public TenantContext<TTenant> TenantContext { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TTenant Tenant { get; set; }
    }
}