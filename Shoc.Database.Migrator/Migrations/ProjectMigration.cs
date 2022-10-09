using System;
using FluentMigrator;

namespace Shoc.Database.Migrator.Migrations
{
    /// <summary>
    /// Project Essentials Migration
    /// </summary>
    [Migration(20221009192303, TransactionBehavior.Default, "Add project essentials")]
    public class ProjectMigration : Migration
    {
        /// <summary>
        /// Migrate database up
        /// </summary>
        public override void Up()
        {
            this.Execute.Script("Migrations.Sql/20221009192303_ProjectMigration.sql");
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