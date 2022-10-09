using System;
using FluentMigrator;

namespace Shoc.Database.Migrator.Migrations
{
    /// <summary>
    /// Builder Essentials Migration
    /// </summary>
    [Migration(20221009192302, TransactionBehavior.Default, "Add builder essentials")]
    public class PaymentMigration : Migration
    {
        /// <summary>
        /// Migrate database up
        /// </summary>
        public override void Up()
        {
            this.Execute.Script("Migrations.Sql/20221009192302_BuilderMigration.sql");
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