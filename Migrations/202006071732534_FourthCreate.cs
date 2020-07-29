using System.Data.Entity.Migrations;

namespace Insurance.Migrations
{
    /// <summary>
    /// Fourth create migration class.
    /// </summary>
    public partial class FourthCreate : DbMigration
    {
        /// <summary>
        /// Up changes to the database.
        /// </summary>
        public override void Up()
        {
            DropForeignKey("dbo.Policy", "Client_ID", "dbo.Client");
            DropIndex("dbo.Policy", new[] { "Client_ID" });
            CreateTable(
                "dbo.PolicyClient",
                p => new
                    {
                        Policy_ID = p.Int(nullable: false),
                        Client_ID = p.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Policy_ID, t.Client_ID })
                .ForeignKey("dbo.Policy", t => t.Policy_ID, cascadeDelete: true)
                .ForeignKey("dbo.Client", t => t.Client_ID, cascadeDelete: true)
                .Index(t => t.Policy_ID)
                .Index(t => t.Client_ID);
            
            DropColumn("dbo.Policy", "Client_ID");
        }

        /// <summary>
        /// Down changes to the database.
        /// </summary>
        public override void Down()
        {
            AddColumn("dbo.Policy", "Client_ID", c => c.Int());
            DropForeignKey("dbo.PolicyClient", "Client_ID", "dbo.Client");
            DropForeignKey("dbo.PolicyClient", "Policy_ID", "dbo.Policy");
            DropIndex("dbo.PolicyClient", new[] { "Client_ID" });
            DropIndex("dbo.PolicyClient", new[] { "Policy_ID" });
            DropTable("dbo.PolicyClient");
            CreateIndex("dbo.Policy", "Client_ID");
            AddForeignKey("dbo.Policy", "Client_ID", "dbo.Client", "ID");
        }
    }
}