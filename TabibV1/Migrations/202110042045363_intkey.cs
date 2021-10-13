namespace TabibV1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class intkey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.doctors", "address_AddressId", "dbo.Addresses");
            DropForeignKey("dbo.doctors", "DoctorAppointments_DoctorAppointmentId", "dbo.DoctorAppointments");
            DropIndex("dbo.doctors", new[] { "address_AddressId" });
            DropIndex("dbo.doctors", new[] { "DoctorAppointments_DoctorAppointmentId" });
            DropColumn("dbo.doctors", "address_AddressId");
            DropColumn("dbo.doctors", "DoctorAppointments_DoctorAppointmentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.doctors", "DoctorAppointments_DoctorAppointmentId", c => c.String(maxLength: 128));
            AddColumn("dbo.doctors", "address_AddressId", c => c.String(maxLength: 128));
            CreateIndex("dbo.doctors", "DoctorAppointments_DoctorAppointmentId");
            CreateIndex("dbo.doctors", "address_AddressId");
            AddForeignKey("dbo.doctors", "DoctorAppointments_DoctorAppointmentId", "dbo.DoctorAppointments", "DoctorAppointmentId");
            AddForeignKey("dbo.doctors", "address_AddressId", "dbo.Addresses", "AddressId");
        }
    }
}
