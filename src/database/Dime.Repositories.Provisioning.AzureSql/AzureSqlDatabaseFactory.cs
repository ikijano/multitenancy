using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Models;
using Microsoft.Azure.Management.Sql;
using Microsoft.Azure.Management.Sql.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;
using System;
using System.Threading.Tasks;

namespace Dime.Repositories
{
    /// <summary>
    ///
    /// </summary>
    public class AzureSqlDatabaseFactory : IDatabaseFactory
    {
        #region Constructor

        /// <summary>
        ///
        /// </summary>
        public AzureSqlDatabaseFactory()
        {
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        ///
        /// </summary>
        private AzureApplicationCredentials Credentials { get; set; }

        /// <summary>
        ///
        /// </summary>
        private string ResourceGroupName { get; set; }

        /// <summary>
        ///
        /// </summary>
        private string ResourceGrouplocation { get; set; }

        ///
        private string Serverlocation => ResourceGrouplocation;

        /// <summary>
        ///
        /// </summary>
        private string ServerName { get; set; }

        /// <summary>
        ///
        /// </summary>
        private string ServerAdmin { get; set; }

        /// <summary>
        ///
        /// </summary>
        private string ServerAdminPassword { get; set; }

        /// <summary>
        ///
        /// </summary>
        private string FirewallRuleName { get; set; }

        /// <summary>
        ///
        /// </summary>
        private string StartIpAddress { get; set; }

        /// <summary>
        ///
        /// </summary>
        private string EndIpAddress { get; set; }

        /// <summary>
        ///
        /// </summary>
        private string DatabaseName { get; set; }

        /// <summary>
        ///
        /// </summary>
        private string DatabaseEdition { get; set; }

        /// <summary>
        ///
        /// </summary>
        private string DatabasePerfLevel { get; set; }

        /// <summary>
        ///
        /// </summary>
        private ResourceManagementClient ResourceMgmtClient { get; set; }

        /// <summary>
        ///
        /// </summary>
        private SqlManagementClient SqlMgmtClient { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="databaseOptions"></param>
        public void Create(DatabaseOptions databaseOptions)
        {
            if (databaseOptions == default(DatabaseOptions))
                throw new ArgumentNullException(nameof(databaseOptions));
            if (databaseOptions as AzureSqlDatabaseOptions == default(AzureSqlDatabaseOptions))
                throw new ArgumentException("Invalid database options class");

            AzureSqlDatabaseOptions databaseSettings = (AzureSqlDatabaseOptions)databaseOptions;
            DatabaseName = databaseSettings.DatabaseName;
            ServerName = databaseSettings.ServerName;
            ServerAdmin = databaseSettings.User;
            ServerAdminPassword = databaseSettings.Password;
            ResourceGroupName = databaseSettings.ResourceGroupName;
            ResourceGrouplocation = databaseSettings.ResourceGrouplocation;
            Credentials = databaseSettings.Credentials;
            FirewallRuleName = databaseSettings.FirewallRule.Name;
            StartIpAddress = databaseSettings.FirewallRule.StartIpAddress;
            EndIpAddress = databaseSettings.FirewallRule.EndIpAddress;
            DatabaseEdition = databaseSettings.DatabaseEdition;
            DatabasePerfLevel = databaseSettings.DatabasePerformanceLevel;

            Connect();

            ResourceGroup resourceGroup = CreateOrUpdateResourceGroup(ResourceMgmtClient, Credentials.SubscriptionId, ResourceGroupName, ResourceGrouplocation);
            Server server = CreateOrUpdateServer(SqlMgmtClient, ResourceGroupName, Serverlocation, ServerName, ServerAdmin, ServerAdminPassword);
            FirewallRule fireWallRule = CreateOrUpdateFirewallRule(SqlMgmtClient, ResourceGroupName, ServerName, FirewallRuleName, StartIpAddress, EndIpAddress);
            Database database = CreateOrUpdateDatabase(SqlMgmtClient, ResourceGroupName, ServerName, DatabaseName, DatabaseEdition, DatabasePerfLevel);
        }

        /// <summary>
        ///
        /// </summary>
        private async Task Connect()
        {
            AuthenticationResult token = await GetToken(Credentials.TenantId, Credentials.ApplicationId, Credentials.ApplicationSecret);
            TokenCredentials accessToken = new TokenCredentials(token.AccessToken);

            ResourceMgmtClient = new ResourceManagementClient(accessToken);
            SqlMgmtClient = new SqlManagementClient(accessToken)
            {
                SubscriptionId = Credentials.SubscriptionId
            };
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="resourceMgmtClient"></param>
        /// <param name="subscriptionId"></param>
        /// <param name="resourceGroupName"></param>
        /// <param name="resourceGroupLocation"></param>
        /// <returns></returns>
        private ResourceGroup CreateOrUpdateResourceGroup(ResourceManagementClient resourceMgmtClient, string subscriptionId, string resourceGroupName, string resourceGroupLocation)
        {
            ResourceGroup resourceGroupParameters = new ResourceGroup(resourceGroupLocation);
            resourceMgmtClient.SubscriptionId = subscriptionId;

            return resourceMgmtClient.ResourceGroups.CreateOrUpdate(resourceGroupName, resourceGroupParameters);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sqlMgmtClient"></param>
        /// <param name="resourceGroupName"></param>
        /// <param name="serverLocation"></param>
        /// <param name="serverName"></param>
        /// <param name="serverAdmin"></param>
        /// <param name="serverAdminPassword"></param>
        /// <returns></returns>
        private Server CreateOrUpdateServer(SqlManagementClient sqlMgmtClient, string resourceGroupName, string serverLocation, string serverName, string serverAdmin, string serverAdminPassword)
        {
            Server serverParameters = new Server
            {
                Location = serverLocation,
                AdministratorLogin = serverAdmin,
                AdministratorLoginPassword = serverAdminPassword,
                Version = "12.0"
            };

            return sqlMgmtClient.Servers.CreateOrUpdate(resourceGroupName, serverName, serverParameters);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sqlMgmtClient"></param>
        /// <param name="resourceGroupName"></param>
        /// <param name="serverName"></param>
        /// <param name="firewallRuleName"></param>
        /// <param name="startIpAddress"></param>
        /// <param name="endIpAddress"></param>
        /// <returns></returns>
        private FirewallRule CreateOrUpdateFirewallRule(SqlManagementClient sqlMgmtClient, string resourceGroupName, string serverName, string firewallRuleName, string startIpAddress, string endIpAddress)
        {
            FirewallRule firewallParameters = new FirewallRule
            {
                StartIpAddress = startIpAddress,
                EndIpAddress = endIpAddress
            };

            return sqlMgmtClient.FirewallRules.CreateOrUpdate(resourceGroupName, serverName, firewallRuleName, firewallParameters);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sqlMgmtClient"></param>
        /// <param name="resourceGroupName"></param>
        /// <param name="serverName"></param>
        /// <param name="databaseName"></param>
        /// <param name="databaseEdition"></param>
        /// <param name="databasePerfLevel"></param>
        /// <returns></returns>
        private Database CreateOrUpdateDatabase(SqlManagementClient sqlMgmtClient, string resourceGroupName, string serverName, string databaseName, string databaseEdition, string databasePerfLevel)
        {
            // Retrieve the server that will host this database
            Server currentServer = sqlMgmtClient.Servers.Get(resourceGroupName, serverName);

            // Create a database: configure create or update parameters and properties explicitly
            Database newDatabaseParameters = new Database
            {
                Location = currentServer.Location,
                CreateMode = CreateMode.Default,
                Edition = databaseEdition,
                RequestedServiceObjectiveName = databasePerfLevel
            };

            return sqlMgmtClient.Databases.CreateOrUpdate(resourceGroupName, serverName, databaseName, newDatabaseParameters);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="applicationId"></param>
        /// <param name="applicationSecret"></param>
        /// <returns></returns>
        private async Task<AuthenticationResult> GetToken(string tenantId, string applicationId, string applicationSecret)
        {
            AuthenticationContext authContext = new AuthenticationContext("https://login.windows.net/" + tenantId);
            return await authContext.AcquireTokenAsync("https://management.core.windows.net/", new ClientCredential(applicationId, applicationSecret));
        }

        #endregion Methods
    }
}