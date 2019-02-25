namespace SinaExpoBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Prices",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CenterType = c.Int(),
                        FestivalAxesId = c.Long(),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FestivalAxes", t => t.FestivalAxesId)
                .Index(t => t.CenterType)
                .Index(t => t.FestivalAxesId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Prices", "FestivalAxesId", "dbo.FestivalAxes");
            DropIndex("dbo.Prices", new[] { "FestivalAxesId" });
            DropIndex("dbo.Prices", new[] { "CenterType" });
            DropTable("dbo.Prices");
        }
    }
}
