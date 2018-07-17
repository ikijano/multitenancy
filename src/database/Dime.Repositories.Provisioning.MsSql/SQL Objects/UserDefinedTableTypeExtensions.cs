using Microsoft.SqlServer.Management.Smo;

namespace Dime.Repositories
{
    internal static class UserDefinedTableTypeExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="userDefinedTableType"></param>
        /// <param name="schema"></param>
        /// <returns></returns>
        internal static UserDefinedTableType Clone(this UserDefinedTableType userDefinedTableType, string schema)
        {
            UserDefinedTableType newTableType = new UserDefinedTableType(userDefinedTableType.Parent, userDefinedTableType.Name, schema)
            {
                IsMemoryOptimized = userDefinedTableType.IsMemoryOptimized,
                UserData = userDefinedTableType.UserData,
                Nullable = userDefinedTableType.Nullable,
                IsUserDefined = userDefinedTableType.IsUserDefined
            };

            foreach (Column column in userDefinedTableType.Columns)
                newTableType.Columns.Add(column.Clone(newTableType));

            return newTableType;
        }
    }
}