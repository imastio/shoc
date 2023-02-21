using System;
using FluentMigrator;

namespace Shoc.Database.Migrator.Migrations
{
    /// <summary>
    /// Executor Essentials Migration
    /// </summary>
    [Migration(20221009192304, TransactionBehavior.Default, "Add executor essentials")]
    public class ExecutorMigration : Migration
    {
        /// <summary>
        /// Migrate database up
        /// </summary>
        public override void Up()
        {
            this.Execute.Script("Migrations.Sql/20221009192304_ExecutorMigration.sql");
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