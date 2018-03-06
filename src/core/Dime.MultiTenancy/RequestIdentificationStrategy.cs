using System.Collections.Generic;

namespace Dime.MultiTenancy
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="environment"></param>
    /// <returns></returns>
    public delegate string RequestIdentificationStrategy(IDictionary<string, object> environment);
}