namespace GameStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration_v3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "IsActived");
            DropColumn("dbo.Users", "ActivationCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "ActivationCode", c => c.Guid(nullable: false));
            AddColumn("dbo.Users", "IsActived", c => c.Boolean(nullable: false));
        }
    }
}
