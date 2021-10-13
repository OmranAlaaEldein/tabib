namespace tabib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addartical : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.articals",
                c => new
                    {
                        articalsID = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.articalsID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.articals");
        }
    }
}
