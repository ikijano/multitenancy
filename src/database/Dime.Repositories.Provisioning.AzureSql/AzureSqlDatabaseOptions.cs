namespace Dime.Repositories
{
    /// <summary>
    ///
    /// </summary>
    public class AzureSqlDatabaseOptions : DatabaseOptions
    {
        /// <summary>
        ///
        /// </summary>
        public string DatabaseEdition { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string DatabasePerformanceLevel { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string ResourceGroupName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string ResourceGrouplocation { get; set; }

        /// <summary>
        ///
        /// </summary>
        public AzureApplicationCredentials Credentials { get; set; }

        /// <summary>
        ///
        /// </summary>
        public AzureFirewallRule FirewallRule { get; set; }
    }
}