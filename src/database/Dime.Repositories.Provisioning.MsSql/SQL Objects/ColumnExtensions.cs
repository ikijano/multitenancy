using Microsoft.SqlServer.Management.Smo;

namespace Dime.Repositories
{
    internal static class ColumnExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="userDefinedTableType"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        internal static Column Clone(this Column userDefinedTableType, SqlSmoObject parent)
        {
            return new Column(parent, userDefinedTableType.Name)
            {
                Collation = userDefinedTableType.Collation,
                Computed = userDefinedTableType.Computed,
                ComputedText = userDefinedTableType.ComputedText,
                DataType = userDefinedTableType.DataType,
                Default = userDefinedTableType.Default,
                DefaultSchema = userDefinedTableType.DefaultSchema,
                Identity = userDefinedTableType.Identity,
                IdentityIncrement = userDefinedTableType.IdentityIncrement,
                IdentitySeed = userDefinedTableType.IdentitySeed,
                IsColumnSet = userDefinedTableType.IsColumnSet,
                IsPersisted = userDefinedTableType.IsPersisted,
                IsSparse = userDefinedTableType.IsSparse,
                NotForReplication = userDefinedTableType.NotForReplication,
                RowGuidCol = userDefinedTableType.RowGuidCol,
                UserData = userDefinedTableType.UserData,
                RuleSchema = userDefinedTableType.RuleSchema,
                Rule = userDefinedTableType.Rule,
                Nullable = userDefinedTableType.Nullable,
                IsFileStream = userDefinedTableType.IsFileStream
            };
        }
    }
}