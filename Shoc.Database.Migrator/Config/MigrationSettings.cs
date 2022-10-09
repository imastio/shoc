namespace Shoc.Database.Migrator.Config
{
    /// <summary>
    /// The set of migration settings
    /// </summary>
    public class MigrationSettings
    {
        /// <summary>
        /// Indicates if migration should happen on app startup
        /// </summary>
        public bool MigrateOnStartup { get; set; }

        /// <summary>
        /// Stop immediately after migration
        /// </summary>
        public bool StopOnMigrate { get; set; }
    }
}