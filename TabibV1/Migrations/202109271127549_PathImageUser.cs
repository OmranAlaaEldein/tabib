namespace TabibV1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PathImageUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "PathOfImage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "PathOfImage");
        }
    }
}
