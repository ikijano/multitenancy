namespace Dime.Repositories
{
    /// <summary>
    ///
    /// </summary>
    public abstract class DatabaseOptions
    {
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        protected DatabaseOptions()
        {

        }

        #endregion Constructor

        #region Properties

        /// <summary>
        ///
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string User { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Password { get; set; }

        #endregion Properties
    }
}