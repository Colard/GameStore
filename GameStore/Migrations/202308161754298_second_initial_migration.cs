namespace GameStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class second_initial_migration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Login", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Login");
        }
    }
}
