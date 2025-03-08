using System;
using FluentMigrator;

namespace Shoc.Database.Migrator.Migrations;

/// <summary>
/// Job Management Migration
/// </summary>
[Migration(11, TransactionBehavior.Default, "Add job management essentials")]
public class V11__JobsMigration : Migration
{
    /// <summary>
    /// Migrate database up
    /// </summary>
    public override void Up()
    {
        this.Execute.Script("Migrations.Sql/V11__JobsMigration.sql");
    }

    /// <summary>
    /// Migrate database down
    /// </summary>
    public override void Down()
    {
        throw new NotImplementedException();
    }
}
