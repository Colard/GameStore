namespace GameStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v47 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "ConfirmPassword");
            DropColumn("dbo.Users", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Users", "ConfirmPassword", c => c.String());
        }
    }
}
