using FluentMigrator.Runner.VersionTableInfo;

namespace Shoc.Database.Migrator.Core;

/// <summary>
/// The metadata for version table
/// </summary>
[VersionTableMetaData]
public class VersionTableMetadata : IVersionTableMetaData
{
    /// <summary>
    /// The application context
    /// </summary>
    public object ApplicationContext { get; set; }

    /// <summary>
    /// Indicates if owns the schema
    /// </summary>
    public bool OwnsSchema => false;

    /// <summary>
    /// Indicates custom schema name
    /// </summary>
    public string SchemaName => string.Empty;

    /// <summary>
    /// The table name for metadata
    /// </summary>
    public string TableName => "gen_migrations";

    /// <summary>
    /// The version column name
    /// </summary>
    public string ColumnName => "Version";

    /// <summary>
    /// The description column name
    /// </summary>
    public string DescriptionColumnName => "Description";

    /// <summary>
    /// An index for migration name uniqueness
    /// </summary>
    public string UniqueIndexName => "Migration_Name_UNIQUE";

    /// <summary>
    /// The name of applied-on attribute
    /// </summary>
    public string AppliedOnColumnName => "AppliedOn";

    /// <summary>
    /// Indicates if it should create table with primary key
    /// </summary>
    public bool CreateWithPrimaryKey => false;
}
