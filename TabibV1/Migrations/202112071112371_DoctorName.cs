namespace TabibV1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DoctorName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.appointments", "DoctorName", c => c.String(maxLength: 50));
            AddColumn("dbo.consulations", "IsReading", c => c.Boolean(nullable: false));
            AddColumn("dbo.consulations", "DoctorName", c => c.String(maxLength: 50));
            DropColumn("dbo.consulations", "IsReaing");
        }
        
        public override void Down()
        {
            AddColumn("dbo.consulations", "IsReaing", c => c.Boolean(nullable: false));
            DropColumn("dbo.consulations", "DoctorName");
            DropColumn("dbo.consulations", "IsReading");
            DropColumn("dbo.appointments", "DoctorName");
        }
    }
}
