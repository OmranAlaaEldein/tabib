namespace tabib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reqimageartical : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.articals", "PathOfImage", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.articals", "PathOfImage", c => c.String(nullable: false));
        }
    }
}
