namespace GameStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v41 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductImages", "Url", c => c.Binary(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductImages", "Url");
        }
    }
}
