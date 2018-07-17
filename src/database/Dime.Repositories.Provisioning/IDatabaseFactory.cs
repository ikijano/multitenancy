namespace Dime.Repositories
{
    /// <summary>
    ///
    /// </summary>
    public interface IDatabaseFactory
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="databaseOptions"></param>
        void Create(DatabaseOptions databaseOptions);
    }
}