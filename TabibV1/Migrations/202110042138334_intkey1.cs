namespace TabibV1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class intkey1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Addresses", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.DoctorAppointments", "Id", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.Addresses");
            AddPrimaryKey("dbo.Addresses", "Id");
            DropPrimaryKey("dbo.DoctorAppointments");
            AddPrimaryKey("dbo.DoctorAppointments", "Id");
            DropColumn("dbo.Addresses", "AddressId");
            DropColumn("dbo.DoctorAppointments", "DoctorAppointmentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DoctorAppointments", "DoctorAppointmentId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Addresses", "AddressId", c => c.String(nullable: false, maxLength: 128));
            DropPrimaryKey("dbo.DoctorAppointments");
            AddPrimaryKey("dbo.DoctorAppointments", "DoctorAppointmentId");
            DropPrimaryKey("dbo.Addresses");
            AddPrimaryKey("dbo.Addresses", "AddressId");
            DropColumn("dbo.DoctorAppointments", "Id");
            DropColumn("dbo.Addresses", "Id");
        }
    }
}
