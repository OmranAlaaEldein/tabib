namespace tabib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class countvisit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfile", "countVisit", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfile", "countVisit");
        }
    }
}
