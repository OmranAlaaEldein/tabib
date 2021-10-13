namespace tabib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.consulations",
                c => new
                    {
                        articalsID = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.articalsID);
            
            CreateTable(
                "dbo.articals",
                c => new
                    {
                        articalsID = c.Int(nullable: false, identity: true),
                        DateOfarticals = c.DateTime(),
                    })
                .PrimaryKey(t => t.articalsID);
            
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserProfile");
            DropTable("dbo.articals");
            DropTable("dbo.consulations");
        }
    }
}
