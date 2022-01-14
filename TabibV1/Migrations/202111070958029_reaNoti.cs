namespace TabibV1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reaNoti : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.consulations", "IsReaing", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.consulations", "IsReaing");
        }
    }
}
