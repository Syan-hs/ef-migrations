namespace EFMigrations.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DealerLocationRelationShipAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DealerLocations",
                c => new
                    {
                        DealerId = c.Int(nullable: false),
                        AddressLine1 = c.String(),
                        AddressLine2 = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Country = c.String(),
                    })
                .PrimaryKey(t => t.DealerId)
                .ForeignKey("dbo.Dealers", t => t.DealerId)
                .Index(t => t.DealerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DealerLocations", "DealerId", "dbo.Dealers");
            DropIndex("dbo.DealerLocations", new[] { "DealerId" });
            DropTable("dbo.DealerLocations");
        }
    }
}
