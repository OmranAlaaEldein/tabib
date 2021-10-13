namespace tabib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class visitClass : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Visiting",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IpAddressVisitor = c.String(),
                        countVisit = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Visiting");
        }
    }
}
