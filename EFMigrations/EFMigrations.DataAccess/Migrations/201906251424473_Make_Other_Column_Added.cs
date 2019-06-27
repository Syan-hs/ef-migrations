namespace EFMigrations.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Make_Other_Column_Added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Makes", "Other", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Makes", "Other");
        }
    }
}
