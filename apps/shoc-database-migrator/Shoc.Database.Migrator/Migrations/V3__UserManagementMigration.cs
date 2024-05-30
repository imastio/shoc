using System;
using FluentMigrator;

namespace Shoc.Database.Migrator.Migrations;

/// <summary>
/// User Management Migration
/// </summary>
[Migration(3, TransactionBehavior.Default, "Add user management essentials")]
public class V3__UserManagementMigration : Migration
{
    /// <summary>
    /// Migrate database up
    /// </summary>
    public override void Up()
    {
        this.Execute.Script("Migrations.Sql/V3__UserManagementMigration.sql");
    }

    /// <summary>
    /// Migrate database down
    /// </summary>
    public override void Down()
    {
        throw new NotImplementedException();
    }
}
