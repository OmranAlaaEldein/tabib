namespace tabib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class datetimeconsultionallownull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.consulations", "DateOfreply", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.consulations", "DateOfreply", c => c.DateTime(nullable: false));
        }
    }
}
