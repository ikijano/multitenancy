using Microsoft.SqlServer.Management.Smo;
using System.Linq;

namespace Dime.Repositories
{
    /// <summary>
    ///
    /// </summary>
    internal static class UserDefinedFunctionCollectionExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="userDefinedFunctions"></param>
        /// <param name="sourceSchema"></param>
        /// <param name="targetSchema"></param>
        internal static void ModifySchema(this UserDefinedFunctionCollection userDefinedFunctions, string sourceSchema, string targetSchema)
        {
            while (userDefinedFunctions.Cast<UserDefinedFunction>().Count(x => x.Schema == sourceSchema) > 0)
            {
                int i = 0;
                userDefinedFunctions.ModifySchema(i, sourceSchema, targetSchema);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="userDefinedFunctions"></param>
        /// <param name="i"></param>
        /// <param name="sourceSchema"></param>
        /// <param name="targetSchema"></param>
        private static void ModifySchema(this UserDefinedFunctionCollection userDefinedFunctions, int i, string sourceSchema, string targetSchema)
        {
            UserDefinedFunction userDefinedFunction = userDefinedFunctions[i];
            if (userDefinedFunction.Schema.Equals(sourceSchema))
            {
                userDefinedFunction.ChangeSchema(targetSchema);
                userDefinedFunction.Alter();
            }
            else
            {
                i += 1;
                userDefinedFunctions.ModifySchema(i, sourceSchema, targetSchema);
            }
        }
    }
}