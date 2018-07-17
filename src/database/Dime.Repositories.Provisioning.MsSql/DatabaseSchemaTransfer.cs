using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Dime.Repositories
{
    /// <summary>
    ///
    /// </summary>
    public class DatabaseSchemaTransfer : Transfer
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="databaseOptions"></param>
        public DatabaseSchemaTransfer(MsSqlDatabaseOptions databaseOptions)
        {
            // Set the DB properties
            SourceDatabase = databaseOptions.SourceDatabase;
            SourceServer = databaseOptions.SourceServer;
            DestinationServer = databaseOptions.ServerName;
            DestinationDatabase = databaseOptions.DatabaseName;
            TargetSchema = databaseOptions.TargetSchema;
            SourceSchema = databaseOptions.SourceSchema;

            // Set the transfer properties
            CopyAllObjects = false;
            CopyAllStoredProcedures = false;
            CopyAllUserDefinedFunctions = false;
            CopyAllTables = false;
            CopyAllSchemas = false;
            CopyData = false;
            CopySchema = true;
            CopyAllViews = false;
            CopyAllSqlAssemblies = false;

            // Set the database to the source db
            Database = SourceDb;

            CompileObjectMigrationList();

            // Copy all constraints and keys
            Options.DriAll = true;
        }

        #region Properties

        /// <summary>
        ///
        /// </summary>
        public bool CreateDatabaseIfNotExists { get; set; }

        /// <summary>
        /// The name of the target schema
        /// </summary>
        public string TargetSchema { get; set; }

        /// <summary>
        ///
        /// </summary>
        public Database TargetDb
        {
            get
            {
                Server targetDbServer = new Server(new ServerConnection(DestinationServer));
                return targetDbServer.Databases[DestinationDatabase];
            }
        }

        /// <summary>
        /// The root database server that contains the database that need to be transferred
        /// </summary>
        public string SourceServer { get; set; }

        /// <summary>
        /// The root database that contains the objects that need to be transferred
        /// </summary>
        public string SourceDatabase { get; set; }

        /// <summary>
        ///
        /// </summary>
        private Database SourceDb
        {
            get
            {
                Server server = new Server(new ServerConnection(SourceServer));
                return server.Databases[SourceDatabase];
            }
        }

        /// <summary>
        /// The name of the schema which needs to be transferred
        /// </summary>
        public string SourceSchema { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool CanMigrate => !TargetDb.HasSchema(TargetSchema);

        #endregion Properties

        #region Methods

        /// <summary>
        ///
        /// </summary>
        public void Configure()
        {
            if (TargetDb == null && CreateDatabaseIfNotExists)
            {
                Server server = new Server(new ServerConnection(SourceServer));
                Database db = new Database(server, DestinationDatabase);
                db.Create();
            }
        }

        /// <summary>
        /// Updates the objects from the source schema that were transferred to the new database with the requested new schema
        /// </summary>
        public void UpdateSchema()
        {
            // Set the database to the target db
            Database = TargetDb;

            // Create schema if not exists
            if (!Database.HasSchema(TargetSchema))
            {
                Schema schema = new Schema(Database, TargetSchema);
                schema.Create();
            }

            Database.Tables.ModifySchema(SourceSchema, TargetSchema);
            Database.StoredProcedures.ModifySchema(SourceSchema, TargetSchema);
            Database.Views.ModifySchema(SourceSchema, TargetSchema);
            Database.UserDefinedFunctions.ModifySchema(SourceSchema, TargetSchema);
            Database.UserDefinedTypes.ModifySchema(SourceSchema, TargetSchema);
            Database.UserDefinedDataTypes.ModifySchema(SourceSchema, TargetSchema);
            Database.UserDefinedTableTypes.ModifySchema(SourceSchema, TargetSchema);

            Database.Refresh();
            Database.StoredProcedures.UpdateSchemaReferences(Database.UserDefinedFunctions, TargetSchema);
        }

        /// <summary>
        /// Filters the objects that need to be included in the transfer.
        /// </summary>
        private void CompileObjectMigrationList()
        {
            foreach (Table tb in Database.Tables)
                if (tb.IsSystemObject == false && !tb.Name.Contains("__refactorlog") && tb.Schema.Contains(SourceSchema))
                    ObjectList.Add(tb);

            foreach (ScriptSchemaObjectBase tb in Database.StoredProcedures)
                if (tb.Schema.Contains(SourceSchema))
                    ObjectList.Add(tb);

            foreach (ScriptSchemaObjectBase tb in Database.Views)
                if (tb.Schema.Contains(SourceSchema))
                    ObjectList.Add(tb);

            foreach (UserDefinedFunction tb in Database.UserDefinedFunctions)
                if (tb.Schema.Contains(SourceSchema) && string.IsNullOrEmpty(tb.AssemblyName))
                    ObjectList.Add(tb);

            foreach (ScriptSchemaObjectBase tb in Database.UserDefinedTypes)
                if (tb.Schema.Contains(SourceSchema))
                    ObjectList.Add(tb);

            foreach (ScriptSchemaObjectBase tb in Database.UserDefinedDataTypes)
                if (tb.Schema.Contains(SourceSchema))
                    ObjectList.Add(tb);

            foreach (ScriptSchemaObjectBase tb in Database.UserDefinedTableTypes)
                if (tb.Schema.Contains(SourceSchema))
                    ObjectList.Add(tb);
        }

        #endregion Methods
    }
}