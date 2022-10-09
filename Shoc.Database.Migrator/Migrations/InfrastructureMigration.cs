using System;
using FluentMigrator;

namespace Shoc.Database.Migrator.Migrations
{
    /// <summary>
    /// Infrastructure Migration
    /// </summary>
    [Migration(20221009192300, TransactionBehavior.Default, "Add infrastructure tables")]
    public class InfrastructureMigration : Migration
    {
        /// <summary>
        /// Migrate database up
        /// </summary>
        public override void Up()
        {
            this.Execute.Script("Migrations.Sql/20221009192300_InfrastructureMigration.sql");
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