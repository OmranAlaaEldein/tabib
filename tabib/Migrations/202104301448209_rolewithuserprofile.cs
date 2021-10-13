namespace tabib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rolewithuserprofile : DbMigration
    {
        public override void Up()
        {
           // CreateTable(
             //   "dbo.RoleUserProfiles",
               // c => new
                 //   {
                   //     Role_RoleId = c.Int(nullable: false),
                    //    UserProfile_UserId = c.Int(nullable: false),
                    //})
              //  .PrimaryKey(t => new { t.Role_RoleId, t.UserProfile_UserId })
              //  .ForeignKey("dbo.webpages_Roles", t => t.Role_RoleId, cascadeDelete: true)
              //  .ForeignKey("dbo.UserProfile", t => t.UserProfile_UserId, cascadeDelete: true)
               // .Index(t => t.Role_RoleId)
           //     .Index(t => t.UserProfile_UserId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.RoleUserProfiles", new[] { "UserProfile_UserId" });
            DropIndex("dbo.RoleUserProfiles", new[] { "Role_RoleId" });
            DropForeignKey("dbo.RoleUserProfiles", "UserProfile_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.RoleUserProfiles", "Role_RoleId", "dbo.webpages_Roles");
            DropTable("dbo.RoleUserProfiles");
        }
    }
}
