namespace SinaExpoBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Groups", "MembersGrade", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Groups", "MembersGrade", c => c.Int(nullable: false));
        }
    }
}
