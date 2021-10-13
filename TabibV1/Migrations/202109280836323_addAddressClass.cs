namespace TabibV1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAddressClass : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.doctors", "address");
        }
        
        public override void Down()
        {
            AddColumn("dbo.doctors", "address", c => c.String());
        }
    }
}
