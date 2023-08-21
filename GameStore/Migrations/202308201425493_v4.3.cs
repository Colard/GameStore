namespace GameStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v43 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "FullName", c => c.String(nullable: false));
            AlterColumn("dbo.Orders", "Phone", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "Phone", c => c.Int(nullable: false));
            AlterColumn("dbo.Orders", "FullName", c => c.Int(nullable: false));
        }
    }
}
