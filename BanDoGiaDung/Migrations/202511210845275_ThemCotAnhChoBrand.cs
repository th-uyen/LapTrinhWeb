namespace BanDoGiaDung.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThemCotAnhChoBrand : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Brand", "image", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Brand", "image");
        }
    }
}
