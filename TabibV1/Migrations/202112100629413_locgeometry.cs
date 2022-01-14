namespace TabibV1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Spatial;
    
    public partial class locgeometry : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Addresses", "Location", c => c.Geography());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Addresses", "Location");
        }
    }
}
