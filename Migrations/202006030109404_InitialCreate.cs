namespace Insurance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Client",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Policy",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        ValidityStart = c.DateTime(nullable: false),
                        Price = c.Double(nullable: false),
                        RiskType = c.Int(nullable: false),
                        Client_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Client", t => t.Client_ID)
                .Index(t => t.Client_ID);
            
            CreateTable(
                "dbo.Coverage",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Percentage = c.Double(nullable: false),
                        Period = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CoveragePolicy",
                c => new
                    {
                        Coverage_ID = c.Int(nullable: false),
                        Policy_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Coverage_ID, t.Policy_ID })
                .ForeignKey("dbo.Coverage", t => t.Coverage_ID, cascadeDelete: true)
                .ForeignKey("dbo.Policy", t => t.Policy_ID, cascadeDelete: true)
                .Index(t => t.Coverage_ID)
                .Index(t => t.Policy_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CoveragePolicy", "Policy_ID", "dbo.Policy");
            DropForeignKey("dbo.CoveragePolicy", "Coverage_ID", "dbo.Coverage");
            DropForeignKey("dbo.Policy", "Client_ID", "dbo.Client");
            DropIndex("dbo.CoveragePolicy", new[] { "Policy_ID" });
            DropIndex("dbo.CoveragePolicy", new[] { "Coverage_ID" });
            DropIndex("dbo.Policy", new[] { "Client_ID" });
            DropTable("dbo.CoveragePolicy");
            DropTable("dbo.Coverage");
            DropTable("dbo.Policy");
            DropTable("dbo.Client");
        }
    }
}
