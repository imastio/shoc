using System;
using FluentMigrator;

namespace Shoc.Database.Migrator.Migrations;

/// <summary>
/// Identity Server Migration
/// </summary>
[Migration(2, TransactionBehavior.Default, "Add essentials for Identity Server")]
public class V2__IdentityServerMigration : Migration
{
    /// <summary>
    /// Migrate database up
    /// </summary>
    public override void Up()
    {
        this.Execute.Script("Migrations.Sql/V2__IdentityServerMigration.sql");
    }

    /// <summary>
    /// Migrate database down
    /// </summary>
    public override void Down()
    {
        throw new NotImplementedException();
    }
}
