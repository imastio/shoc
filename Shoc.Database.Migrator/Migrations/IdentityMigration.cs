using System;
using FluentMigrator;

namespace Shoc.Database.Migrator.Migrations
{
    /// <summary>
    /// Identity Essentials Migration
    /// </summary>
    [Migration(20221009192301, TransactionBehavior.Default, "Add identity essentials")]
    public class IdentityMigration : Migration
    {
        /// <summary>
        /// Migrate database up
        /// </summary>
        public override void Up()
        {
            this.Execute.Script("Migrations.Sql/20221009192301_IdentityMigration.sql");
        }

        /// <summary>
        /// Migrate database down
        /// </summary>
        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}