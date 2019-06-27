namespace EFMigrations.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Dealer_Make_RelationShipAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DealerMakeMapping",
                c => new
                    {
                        DealerId = c.Int(nullable: false),
                        MakeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DealerId, t.MakeId })
                .ForeignKey("dbo.Dealers", t => t.DealerId, cascadeDelete: true)
                .ForeignKey("dbo.Makes", t => t.MakeId, cascadeDelete: true)
                .Index(t => t.DealerId)
                .Index(t => t.MakeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DealerMakeMapping", "MakeId", "dbo.Makes");
            DropForeignKey("dbo.DealerMakeMapping", "DealerId", "dbo.Dealers");
            DropIndex("dbo.DealerMakeMapping", new[] { "MakeId" });
            DropIndex("dbo.DealerMakeMapping", new[] { "DealerId" });
            DropTable("dbo.DealerMakeMapping");
        }
    }
}
