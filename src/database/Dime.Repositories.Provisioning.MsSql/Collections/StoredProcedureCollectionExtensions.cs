using Microsoft.SqlServer.Management.Smo;
using System.Collections.Generic;
using System.Linq;

namespace Dime.Repositories
{
    public static class StoredProcedureCollectionExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="storedProcedures"></param>
        /// <param name="sourceSchema"></param>
        /// <param name="targetSchema"></param>
        internal static void ModifySchema(this StoredProcedureCollection storedProcedures, string sourceSchema, string targetSchema)
        {
            while (storedProcedures.Cast<StoredProcedure>().Where(x => !x.Name.Contains("sp_")).Count(x => x.Schema == sourceSchema) > 0)
            {
                int i = 0;
                storedProcedures.ModifySchema(i, sourceSchema, targetSchema);
            }
        }

        ///  <summary>
        /// 
        ///  </summary>
        ///  <param name="storedProcedures"></param>
        /// <param name="userDefinedFunctions"></param>
        /// <param name="targetSchema"></param>
        public static void UpdateSchemaReferences(this StoredProcedureCollection storedProcedures, UserDefinedFunctionCollection userDefinedFunctions, string targetSchema)
        {
            IEnumerable<string> userDefinedFunctionNames = userDefinedFunctions.Cast<UserDefinedFunction>().Select(x => x.Name);
            foreach (StoredProcedure storedProcedure in storedProcedures.Cast<StoredProcedure>().Where(x => x.Schema.Equals(targetSchema)))
            {
                if (!userDefinedFunctionNames.Any(x => storedProcedure.TextBody.Contains(x)))
                    continue;

                storedProcedure.TextBody = storedProcedure.TextBody.Replace("dbo", targetSchema);
                storedProcedure.TextHeader = storedProcedure.TextHeader.Replace("dbo", targetSchema);
                storedProcedure.Alter();
            }
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="storedProcedures"></param>
        /// <param name="i"></param>
        /// <param name="sourceSchema"></param>
        /// <param name="targetSchema"></param>
        private static void ModifySchema(this StoredProcedureCollection storedProcedures, int i, string sourceSchema, string targetSchema)
        {
            StoredProcedure storedProcedure = storedProcedures[i];
            if (storedProcedure.Schema.Equals(sourceSchema))
            {
                storedProcedure.ChangeSchema(targetSchema);
                storedProcedure.Alter();
            }
            else
            {
                i += 1;
                storedProcedures.ModifySchema(i, sourceSchema, targetSchema);
            }
        }
    }
}