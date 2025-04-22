using System;
using FluentMigrator;

namespace Shoc.Database.Migrator.Migrations;

/// <summary>
/// Job Management Migration
/// </summary>
[Migration(12, TransactionBehavior.Default, "Add job oidc providers essentials")]
public class V12__OidcProvidersMigrator : Migration
{
    /// <summary>
    /// Migrate database up
    /// </summary>
    public override void Up()
    {
        this.Execute.Script("Migrations.Sql/V12__OidcProvidersMigrator.sql");
    }

    /// <summary>
    /// Migrate database down
    /// </summary>
    public override void Down()
    {
        throw new NotImplementedException();
    }
}
