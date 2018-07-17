using System;

namespace Dime.Repositories
{
    /// <summary>
    ///
    /// </summary>
    public class MsSqlDatabaseFactory : IDatabaseFactory
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="databaseOptions"></param>
        public void Create(DatabaseOptions databaseOptions)
        {
            if (databaseOptions == default(DatabaseOptions))
                throw new ArgumentNullException(nameof(databaseOptions));
            if (databaseOptions as MsSqlDatabaseOptions == default(MsSqlDatabaseOptions))
                throw new ArgumentException("Invalid database options class");

            // Cast options object to MSSQL config
            MsSqlDatabaseOptions databaseSettings = (MsSqlDatabaseOptions)databaseOptions;

            // Configure the database, migrate the dbo schema to the target schema and update the schema on the target database
            DatabaseSchemaTransfer migration = new DatabaseSchemaTransfer(databaseSettings);
            if (!migration.CanMigrate)
                throw new Exception("Schema already exists in the database. Please select a unique schema name");

            migration.Configure();
            migration.TransferData();
            migration.UpdateSchema();
        }
    }
}