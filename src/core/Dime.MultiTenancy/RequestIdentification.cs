using Microsoft.Owin;

namespace Dime.MultiTenancy
{
    /// <summary>
    ///
    /// </summary>
    public static class RequestIdentification
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static RequestIdentificationStrategy FromHostname()
        {
            return env => new OwinContext(env).Request.Uri.Host;
        }
    }
}