using System.Linq;
using Imast.DataOps.Api;
using Imast.DataOps.Init;
using MySql.Data.MySqlClient;

namespace Shoc.Data.Sql;

/// <summary>
/// The operations initializer module
/// </summary>
public class OperationsInitializer
{
    /// <summary>
    /// Initializes new instance of data operations
    /// </summary>
    /// <param name="settings">The data source settings</param>
    /// <param name="sources">The set of sources</param>
    /// <returns></returns>
    public static DataOperations Init(DataSourceSettings settings, params string[] sources)
    {
        // create new connection string builder
        var connectionStringBuilder = new MySqlConnectionStringBuilder(settings.ConnectionString)
        {
            Database = settings.Database,
            AllowUserVariables = true
        };

        // build and init new ops object
        var dataOps = DataOperationsBuilder.New()
            .WithConnection(SqlProvider.MySQL, () => new MySqlConnection(connectionStringBuilder.ToString()))
            .WithDefaultProvider(SqlProvider.MySQL)
            .WithSchemaValidation();

        // append all given sources
        dataOps = sources.Aggregate(dataOps, (current, source) => current.WithSource(source));

        // build final data ops
        return dataOps.Build();
    }
}
