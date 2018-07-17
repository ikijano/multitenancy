using Microsoft.SqlServer.Management.Smo;
using System.Linq;

namespace Dime.Repositories
{
    internal static class UserDefinedDataTypeCollectionExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="userDefinedDataTypes"></param>
        /// <param name="sourceSchema"></param>
        /// <param name="targetSchema"></param>
        internal static void ModifySchema(this UserDefinedDataTypeCollection userDefinedDataTypes, string sourceSchema, string targetSchema)
        {
            while (userDefinedDataTypes.Cast<UserDefinedDataType>().Count(x => x.Schema == sourceSchema) > 0)
            {
                int i = 0;
                userDefinedDataTypes.ModifySchema(i, sourceSchema, targetSchema);
            }
        }

        ///  <summary>
        /// 
        ///  </summary>
        ///  <param name="tables"></param>
        /// <param name="i"></param>
        /// <param name="sourceSchema"></param>
        /// <param name="targetSchema"></param>
        private static void ModifySchema(this UserDefinedDataTypeCollection tables, int i, string sourceSchema, string targetSchema)
        {
            UserDefinedDataType tb = tables[i];
            if (tb.Schema.Equals(sourceSchema))
            {
                tb.Schema = targetSchema;
                tb.Alter();
            }
            else
            {
                i += 1;
                tables.ModifySchema(i, sourceSchema, targetSchema);
            }
        }
    }
}