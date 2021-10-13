namespace tabib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteartical : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.articals");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.articals",
                c => new
                    {
                        articalsID = c.Int(nullable: false, identity: true),
                        DateOfarticals = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.articalsID);
            
        }
    }
}
