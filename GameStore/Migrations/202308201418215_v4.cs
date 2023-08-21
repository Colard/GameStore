namespace GameStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "OrderStatusId", "dbo.OrderStatus");
            DropIndex("dbo.Orders", new[] { "OrderStatusId" });
            AddColumn("dbo.Orders", "FullName", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "Phone", c => c.Int(nullable: false));
            AlterColumn("dbo.UserAddresses", "Address", c => c.String(nullable: false, maxLength: 40));
            DropColumn("dbo.Orders", "OrderStatusId");
            DropColumn("dbo.Orders", "Price");
            DropTable("dbo.OrderStatus");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.OrderStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Orders", "Price", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "OrderStatusId", c => c.Int(nullable: false));
            AlterColumn("dbo.UserAddresses", "Address", c => c.String(nullable: false));
            DropColumn("dbo.Orders", "Phone");
            DropColumn("dbo.Orders", "FullName");
            CreateIndex("dbo.Orders", "OrderStatusId");
            AddForeignKey("dbo.Orders", "OrderStatusId", "dbo.OrderStatus", "Id", cascadeDelete: true);
        }
    }
}
