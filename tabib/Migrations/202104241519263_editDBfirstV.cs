namespace tabib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editDBfirstV : DbMigration
    {
        public override void Up()
        {
            //DropPrimaryKey("dbo.consulations", new[] { "articalsID" });
            //DropColumn("dbo.consulations", "articalsID");
            //AddColumn("dbo.consulations", "consulationID", c => c.Int(nullable: false, identity: true));
            //AddPrimaryKey("dbo.consulations", "consulationID");

            RenameColumn("dbo.consulations", "articalsID", "consulationID");
            AddColumn("dbo.consulations", "title", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.consulations", "Question", c => c.String(nullable: false, maxLength: 500));
            AddColumn("dbo.consulations", "DateOfconsulation", c => c.DateTime(nullable: false));
            AddColumn("dbo.consulations", "reply", c => c.String(maxLength: 500));
            AddColumn("dbo.consulations", "Isreplaied", c => c.Boolean(nullable: false));
            AddColumn("dbo.consulations", "DateOfreply", c => c.DateTime(nullable: false));
            AddColumn("dbo.consulations", "PathImage", c => c.String());
            AddColumn("dbo.consulations", "KeyWords", c => c.String(maxLength: 50));
            AddColumn("dbo.consulations", "UserProfileId", c => c.Int(nullable: false));
            AddColumn("dbo.consulations", "UserProfile_UserId", c => c.Int());
            AddColumn("dbo.UserProfile", "FirstName", c => c.String());
            AddColumn("dbo.UserProfile", "LastName", c => c.String());
            AddColumn("dbo.UserProfile", "email", c => c.String());
            AddColumn("dbo.UserProfile", "DateBirthday", c => c.DateTime(nullable: false));
            AlterColumn("dbo.articals", "DateOfarticals", c => c.DateTime(nullable: false));
            AlterColumn("dbo.UserProfile", "UserName", c => c.String(nullable: false));
            AddForeignKey("dbo.consulations", "UserProfile_UserId", "dbo.UserProfile", "UserId");
            CreateIndex("dbo.consulations", "UserProfile_UserId");
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.consulations", "articalsID", c => c.Int(nullable: false, identity: true));
            DropIndex("dbo.consulations", new[] { "UserProfile_UserId" });
            DropForeignKey("dbo.consulations", "UserProfile_UserId", "dbo.UserProfile");
            DropPrimaryKey("dbo.consulations", new[] { "consulationID" });
            AddPrimaryKey("dbo.consulations", "articalsID");
            AlterColumn("dbo.UserProfile", "UserName", c => c.String());
            AlterColumn("dbo.articals", "DateOfarticals", c => c.DateTime());
            DropColumn("dbo.UserProfile", "DateBirthday");
            DropColumn("dbo.UserProfile", "email");
            DropColumn("dbo.UserProfile", "LastName");
            DropColumn("dbo.UserProfile", "FirstName");
            DropColumn("dbo.consulations", "UserProfile_UserId");
            DropColumn("dbo.consulations", "UserProfileId");
            DropColumn("dbo.consulations", "KeyWords");
            DropColumn("dbo.consulations", "PathImage");
            DropColumn("dbo.consulations", "DateOfreply");
            DropColumn("dbo.consulations", "Isreplaied");
            DropColumn("dbo.consulations", "reply");
            DropColumn("dbo.consulations", "DateOfconsulation");
            DropColumn("dbo.consulations", "Question");
            DropColumn("dbo.consulations", "title");
            DropColumn("dbo.consulations", "consulationID");
        }
    }
}
