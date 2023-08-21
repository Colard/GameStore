namespace GameStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v49 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "Title", c => c.String(nullable: false, maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "Title", c => c.String(nullable: false));
        }
    }
}
