namespace tabib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class failedrtical : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.articals", "titleOfarticls", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.articals", "typeOfarticls", c => c.String());
            AddColumn("dbo.articals", "TextArticles", c => c.String(nullable: false));
            AddColumn("dbo.articals", "PathOfImage", c => c.String(nullable: false));
            AddColumn("dbo.articals", "DateOfarticals", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.articals", "DateOfarticals");
            DropColumn("dbo.articals", "PathOfImage");
            DropColumn("dbo.articals", "TextArticles");
            DropColumn("dbo.articals", "typeOfarticls");
            DropColumn("dbo.articals", "titleOfarticls");
        }
    }
}
