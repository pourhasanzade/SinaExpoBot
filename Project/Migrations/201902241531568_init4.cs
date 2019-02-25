namespace SinaExpoBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Prices", "PriceAmount", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Prices", "PriceAmount");
        }
    }
}
