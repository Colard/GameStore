namespace GameStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v44 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "MiddleName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "MiddleName", c => c.String());
        }
    }
}
