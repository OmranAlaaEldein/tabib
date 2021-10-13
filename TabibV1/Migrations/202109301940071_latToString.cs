namespace TabibV1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class latToString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Addresses", "lang", c => c.String());
            AlterColumn("dbo.Addresses", "lat", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Addresses", "lat", c => c.Int(nullable: false));
            AlterColumn("dbo.Addresses", "lang", c => c.Int(nullable: false));
        }
    }
}
