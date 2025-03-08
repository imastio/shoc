using MySql.Data.MySqlClient;

namespace Shoc.Data.Sql;

/// <summary>
/// Extended operations for sql
/// </summary>
public static class SqlExt
{
    /// <summary>
    /// Build connection string for default data source
    /// </summary>
    /// <param name="settings">The settings</param>
    /// <returns></returns>
    public static string BuildConnectionString(DataSourceSettings settings)
    {
        // create new connection string builder
        return new MySqlConnectionStringBuilder(settings.ConnectionString)
        {
            Database = settings.Database,
            AllowUserVariables = true
        }.ToString();
    }
}