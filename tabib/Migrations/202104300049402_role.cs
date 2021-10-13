namespace tabib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class role : DbMigration
    {
        public override void Up()
        {
            //CreateTable( "dbo.webpages_Roles", c => new {  RoleId = c.Int(nullable: false, identity: true), RoleName = c.String(nullable: false),   }).PrimaryKey(t => t.RoleId);
            
            AlterColumn("dbo.UserProfile", "Email", c => c.String());
            DropColumn("dbo.UserProfile", "FirstName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserProfile", "FirstName", c => c.String());
            AlterColumn("dbo.UserProfile", "email", c => c.String());
            //DropTable("dbo.webpages_Roles");
        }
    }
}
