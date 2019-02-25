namespace SinaExpoBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init5 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Groups", name: "FestivalAxesId", newName: "FestivalAxes_Id");
            RenameIndex(table: "dbo.Groups", name: "IX_FestivalAxesId", newName: "IX_FestivalAxes_Id");
            AddColumn("dbo.Groups", "FestivalAxId", c => c.Long());
            AddColumn("dbo.Groups", "AxTitle", c => c.String(maxLength: 50));
            DropColumn("dbo.Groups", "AxesTitle");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Groups", "AxesTitle", c => c.String(maxLength: 50));
            DropColumn("dbo.Groups", "AxTitle");
            DropColumn("dbo.Groups", "FestivalAxId");
            RenameIndex(table: "dbo.Groups", name: "IX_FestivalAxes_Id", newName: "IX_FestivalAxesId");
            RenameColumn(table: "dbo.Groups", name: "FestivalAxes_Id", newName: "FestivalAxesId");
        }
    }
}
