using System.Data.Entity.Migrations;

namespace Insurance.Migrations
{
    /// <summary>
    /// Second create migration class.
    /// </summary>
    public partial class SecondCreate : DbMigration
    {
        /// <summary>
        /// Up changes to the database.
        /// </summary>
        public override void Up()
        {
            AlterColumn("dbo.Client", "Name", c => c.String(maxLength: 50));
            AlterColumn("dbo.Policy", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Policy", "Description", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Coverage", "Name", c => c.String(maxLength: 50));
        }

        /// <summary>
        /// Down changes to the database.
        /// </summary>
        public override void Down()
        {
            AlterColumn("dbo.Coverage", "Name", c => c.String());
            AlterColumn("dbo.Policy", "Description", c => c.String());
            AlterColumn("dbo.Policy", "Name", c => c.String());
            AlterColumn("dbo.Client", "Name", c => c.String());
        }
    }
}