using Microsoft.SqlServer.Management.Smo;
using System;
using System.Linq;

namespace Dime.Repositories
{
    /// <summary>
    ///
    /// </summary>
    internal static class DatabaseExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="db"></param>
        /// <param name="schema"></param>
        /// <returns></returns>
        internal static bool HasSchema(this Database db, string schema)
            => db.Schemas.Cast<Schema>().Any(y => y.Name.Equals(schema, StringComparison.InvariantCultureIgnoreCase));
    }
}