using System;
using FluentMigrator;

namespace Shoc.Database.Migrator.Migrations;

/// <summary>
/// Quartz Tables Migration
/// </summary>
[Migration(10, TransactionBehavior.Default, "Add quartz tables")]
public class V10__QuartzMigration : Migration
{
    /// <summary>
    /// Migrate database up
    /// </summary>
    public override void Up()
    {
        this.Execute.Script("Migrations.Sql/V10__QuartzMigration.sql");
    }

    /// <summary>
    /// Migrate database down
    /// </summary>
    public override void Down()
    {
        throw new NotImplementedException();
    }
}
