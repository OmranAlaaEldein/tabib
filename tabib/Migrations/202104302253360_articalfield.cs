namespace tabib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class articalfield : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.articals", "titleOfarticls", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.articals", "typeOfarticls", c => c.String());
            AddColumn("dbo.articals", "TextArticles",  c => c.String(nullable: false, maxLength: 5000));
            AddColumn("dbo.articals", "PathOfImage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.articals", "titleOfarticls");
            DropColumn("dbo.articals", "typeOfarticls");
            DropColumn("dbo.articals", "TextArticles");
            DropColumn("dbo.articals", "PathOfImage");

        }
    }
}
