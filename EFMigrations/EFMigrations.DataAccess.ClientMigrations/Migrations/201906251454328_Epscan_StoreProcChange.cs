using System.Collections.Generic;

namespace EFMigrations.DataAccess.ClientMigrations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Epscan_StoreProcChange : BaseMigration
    {
        protected override ExecuteMigrationOption ExecuteMigrationOption
        {
            get
            {
                return new ExecuteMigrationOption()
                {
                    DataBases = new List<string>() { "10075" }
                };
            }
        }

        public override void UpMigration()
        {
            Sql(@"ALTER proc [dbo].[ClientSpecificProc]
                as
                begin

                select 'Epscan'
            end");
        }

        public override void DownBackMigration()
        {
            Sql(@"ALTER proc [dbo].[ClientSpecificProc]
                as
                begin

            select 'Test'
            end");
        }
    }
}
