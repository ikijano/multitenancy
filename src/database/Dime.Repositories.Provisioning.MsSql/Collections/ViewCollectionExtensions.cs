using Microsoft.SqlServer.Management.Smo;
using System.Linq;

namespace Dime.Repositories
{
    internal static class ViewCollectionExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="tables"></param>
        /// <param name="sourceSchema"></param>
        /// <param name="targetSchema"></param>
        internal static void ModifySchema(this ViewCollection tables, string sourceSchema, string targetSchema)
        {
            while (tables.Cast<View>().Count(x => x.Schema == sourceSchema) > 0)
            {
                int i = 0;
                tables.ModifySchema(i, sourceSchema, targetSchema);
            }
        }

        ///  <summary>
        /// 
        ///  </summary>
        ///  <param name="tables"></param>
        /// <param name="i"></param>
        /// <param name="sourceSchema"></param>
        /// <param name="targetSchema"></param>
        private static void ModifySchema(this ViewCollection tables, int i, string sourceSchema, string targetSchema)
        {
            View tb = tables[i];
            if (tb.Schema.Equals(sourceSchema))
            {
                tb.ChangeSchema(targetSchema);
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