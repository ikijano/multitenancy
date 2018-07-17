using Microsoft.SqlServer.Management.Smo;
using System.Linq;

namespace Dime.Repositories
{
    internal static class UserDefinedTableTypeCollectionExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="userDefinedTableTypes"></param>
        /// <param name="sourceSchema"></param>
        /// <param name="targetSchema"></param>
        internal static void ModifySchema(this UserDefinedTableTypeCollection userDefinedTableTypes, string sourceSchema, string targetSchema)
        {
            while (userDefinedTableTypes.Cast<UserDefinedTableType>().Count(x => x.Schema == sourceSchema) > 0)
            {
                int i = 0;
                userDefinedTableTypes.ModifySchema(i, sourceSchema, targetSchema);
            }
        }

        ///  <summary>
        /// 
        ///  </summary>
        ///  <param name="userDefinedTableTypes"></param>
        /// <param name="i"></param>
        /// <param name="sourceSchema"></param>
        /// <param name="targetSchema"></param>
        private static void ModifySchema(this UserDefinedTableTypeCollection userDefinedTableTypes, int i, string sourceSchema, string targetSchema)
        {
            UserDefinedTableType userDefinedTableType = userDefinedTableTypes[i];
            if (userDefinedTableType.Schema.Equals(sourceSchema))
            {
                UserDefinedTableType newTableType = userDefinedTableType.Clone(targetSchema);
                newTableType.Create();
                userDefinedTableType.Drop();
            }
            else
            {
                i += 1;
                userDefinedTableTypes.ModifySchema(i, sourceSchema, targetSchema);
            }
        }
    }
}