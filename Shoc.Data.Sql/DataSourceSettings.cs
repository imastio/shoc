namespace Shoc.Data.Sql
{
    /// <summary>
    /// The data source settings
    /// </summary>
    public class DataSourceSettings
    {
        /// <summary>
        /// The connection string to the target database
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// The initial database name 
        /// </summary>
        public string Database { get; set; }
    }
}