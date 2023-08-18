namespace GameStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration_v4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "MainPhoto", c => c.Binary());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "MainPhoto", c => c.Binary(nullable: false));
        }
    }
}
