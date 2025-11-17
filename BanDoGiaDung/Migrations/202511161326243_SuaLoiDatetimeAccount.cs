namespace BanDoGiaDung.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SuaLoiDatetimeAccount : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Accounts", "create_at", c => c.DateTime(nullable: true));
            AlterColumn("dbo.Accounts", "update_at", c => c.DateTime(nullable: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Accounts", "update_at", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Accounts", "create_at", c => c.DateTime(nullable: false));
        }
    }
}
