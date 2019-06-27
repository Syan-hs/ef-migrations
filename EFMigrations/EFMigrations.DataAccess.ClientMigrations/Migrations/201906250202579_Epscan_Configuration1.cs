using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace EFMigrations.DataAccess.ClientMigrations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Epscan_Configuration1 : BaseMigration
    {
        protected override ExecuteMigrationOption ExecuteMigrationOption => new ExecuteMigrationOption()
        {
            DataBases = new List<string>()
            {
                "TS_10075"
            }
        };

        private readonly string _dealerName = "Epscan_" + DateTime.Now.Ticks.ToString();

        public override void UpMigration()
        {
           Sql( $"INSERT INTO dbo.Dealers (Name,OwnerName) values('{_dealerName}', '{_dealerName}')");
        }

        public override void DownBackMigration()
        {
            Sql($"DELETE FROM dbo.Dealers WHERE Name = '{_dealerName}' ");
        }
    }


}
