namespace Dime.Repositories
{
    /// <summary>
    ///
    /// </summary>
    public class MsSqlDatabaseOptions : DatabaseOptions
    {
        #region Constructor

        /// <summary>
        ///
        /// </summary>
        public MsSqlDatabaseOptions()
        {
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        ///
        /// </summary>
        public string SourceDatabase { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string SourceServer { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string SourceSchema { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string TargetSchema { get; set; }

        #endregion Properties
    }
}