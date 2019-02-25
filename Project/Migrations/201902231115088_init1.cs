namespace SinaExpoBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Buttons",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Code = c.String(),
                        Text = c.String(maxLength: 50),
                        Type = c.Int(nullable: false),
                        ViewType = c.Int(nullable: false),
                        ImageUrl = c.String(maxLength: 250),
                        Row = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                        StateId = c.Int(nullable: false),
                        Data = c.String(maxLength: 500),
                        BehaviourType = c.Int(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.StateId);
            
            CreateTable(
                "dbo.Configs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        LastMessageId = c.String(maxLength: 200),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ExceptionLogs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ChatId = c.String(maxLength: 250),
                        Title = c.String(maxLength: 250),
                        Exception = c.String(),
                        WebException = c.String(),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FestivalAxes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FestivalFields",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FestivalAxesId = c.Long(),
                        Title = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FestivalAxes", t => t.FestivalAxesId)
                .Index(t => t.FestivalAxesId);
            
            CreateTable(
                "dbo.GroupMembers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ChatId = c.String(maxLength: 50),
                        MemberNumber = c.Int(nullable: false),
                        Name = c.String(maxLength: 20),
                        NationalCode = c.String(maxLength: 20),
                        IsDeleted = c.Boolean(nullable: false),
                        GroupId = c.Long(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.GroupId)
                .Index(t => t.ChatId)
                .Index(t => t.MemberNumber)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ChatId = c.String(maxLength: 50),
                        CenterName = c.String(maxLength: 100),
                        CenterType = c.Int(),
                        CenterGenderType = c.Int(),
                        ProvinceId = c.Long(),
                        ProvinceName = c.String(maxLength: 50),
                        Address = c.String(maxLength: 2000),
                        PostalCode = c.String(maxLength: 10),
                        Phone = c.String(maxLength: 20),
                        ManagerName = c.String(maxLength: 100),
                        SupervisorName = c.String(maxLength: 100),
                        SupervisorMobile = c.String(maxLength: 11),
                        SupervisorEmail = c.String(maxLength: 50),
                        ProjectSubject = c.String(),
                        FestivalAxesId = c.Long(),
                        AxesTitle = c.String(maxLength: 50),
                        FestivalFieldId = c.Long(),
                        FieldTitle = c.String(maxLength: 50),
                        FestivalMajorId = c.Long(),
                        MajorTitle = c.String(maxLength: 50),
                        MembersCount = c.Int(),
                        MembersGrade = c.Int(nullable: false),
                        PayDate = c.DateTime(),
                        Price = c.String(maxLength: 20),
                        OrderId = c.Long(),
                        IsFinished = c.Boolean(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FestivalAxes", t => t.FestivalAxesId)
                .ForeignKey("dbo.FestivalFields", t => t.FestivalFieldId)
                .ForeignKey("dbo.FestivalMajors", t => t.FestivalMajorId)
                .ForeignKey("dbo.Orders", t => t.OrderId)
                .ForeignKey("dbo.Provinces", t => t.ProvinceId)
                .Index(t => t.ChatId)
                .Index(t => t.ProvinceId)
                .Index(t => t.FestivalAxesId)
                .Index(t => t.FestivalFieldId)
                .Index(t => t.FestivalMajorId)
                .Index(t => t.OrderId)
                .Index(t => t.IsFinished);
            
            CreateTable(
                "dbo.FestivalMajors",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FieldId = c.Long(),
                        Title = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FestivalFields", t => t.FieldId)
                .Index(t => t.FieldId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ChatId = c.String(maxLength: 50),
                        PaymentToken = c.String(maxLength: 250),
                        Type = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        SettleStatus = c.Int(nullable: false),
                        OrderStatus = c.Int(nullable: false),
                        Amount = c.String(maxLength: 20),
                        PlanId = c.Int(),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Provinces",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserData",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ChatId = c.String(maxLength: 250),
                        StateId = c.Long(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.ChatId, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GroupMembers", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.Groups", "ProvinceId", "dbo.Provinces");
            DropForeignKey("dbo.Groups", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Groups", "FestivalMajorId", "dbo.FestivalMajors");
            DropForeignKey("dbo.FestivalMajors", "FieldId", "dbo.FestivalFields");
            DropForeignKey("dbo.Groups", "FestivalFieldId", "dbo.FestivalFields");
            DropForeignKey("dbo.Groups", "FestivalAxesId", "dbo.FestivalAxes");
            DropForeignKey("dbo.FestivalFields", "FestivalAxesId", "dbo.FestivalAxes");
            DropIndex("dbo.UserData", new[] { "ChatId" });
            DropIndex("dbo.FestivalMajors", new[] { "FieldId" });
            DropIndex("dbo.Groups", new[] { "IsFinished" });
            DropIndex("dbo.Groups", new[] { "OrderId" });
            DropIndex("dbo.Groups", new[] { "FestivalMajorId" });
            DropIndex("dbo.Groups", new[] { "FestivalFieldId" });
            DropIndex("dbo.Groups", new[] { "FestivalAxesId" });
            DropIndex("dbo.Groups", new[] { "ProvinceId" });
            DropIndex("dbo.Groups", new[] { "ChatId" });
            DropIndex("dbo.GroupMembers", new[] { "GroupId" });
            DropIndex("dbo.GroupMembers", new[] { "MemberNumber" });
            DropIndex("dbo.GroupMembers", new[] { "ChatId" });
            DropIndex("dbo.FestivalFields", new[] { "FestivalAxesId" });
            DropIndex("dbo.Buttons", new[] { "StateId" });
            DropTable("dbo.UserData");
            DropTable("dbo.Provinces");
            DropTable("dbo.Orders");
            DropTable("dbo.FestivalMajors");
            DropTable("dbo.Groups");
            DropTable("dbo.GroupMembers");
            DropTable("dbo.FestivalFields");
            DropTable("dbo.FestivalAxes");
            DropTable("dbo.ExceptionLogs");
            DropTable("dbo.Configs");
            DropTable("dbo.Buttons");
        }
    }
}
