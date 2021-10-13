namespace TabibV1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAddressAndAppointment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        AddressId = c.String(nullable: false, maxLength: 128),
                        lang = c.Int(nullable: false),
                        lat = c.Int(nullable: false),
                        textAddress = c.String(),
                    })
                .PrimaryKey(t => t.AddressId);
            
            CreateTable(
                "dbo.DoctorAppointments",
                c => new
                    {
                        DoctorAppointmentId = c.String(nullable: false, maxLength: 128),
                        TmieOfAppointment = c.Int(nullable: false),
                        firstFrom = c.DateTime(nullable: false),
                        firstTo = c.DateTime(nullable: false),
                        secondFrom = c.DateTime(nullable: false),
                        secondTo = c.DateTime(nullable: false),
                        Days = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DoctorAppointmentId);
            
            AddColumn("dbo.doctors", "address_AddressId", c => c.String(maxLength: 128));
            AddColumn("dbo.doctors", "DoctorAppointments_DoctorAppointmentId", c => c.String(maxLength: 128));
            CreateIndex("dbo.doctors", "address_AddressId");
            CreateIndex("dbo.doctors", "DoctorAppointments_DoctorAppointmentId");
            AddForeignKey("dbo.doctors", "address_AddressId", "dbo.Addresses", "AddressId");
            AddForeignKey("dbo.doctors", "DoctorAppointments_DoctorAppointmentId", "dbo.DoctorAppointments", "DoctorAppointmentId");
            DropColumn("dbo.doctors", "TmieOfAppointment");
            DropColumn("dbo.doctors", "firstFrom");
            DropColumn("dbo.doctors", "firstTo");
            DropColumn("dbo.doctors", "secondFrom");
            DropColumn("dbo.doctors", "secondTo");
            DropColumn("dbo.doctors", "Days");
        }
        
        public override void Down()
        {
            AddColumn("dbo.doctors", "Days", c => c.Int(nullable: false));
            AddColumn("dbo.doctors", "secondTo", c => c.DateTime(nullable: false));
            AddColumn("dbo.doctors", "secondFrom", c => c.DateTime(nullable: false));
            AddColumn("dbo.doctors", "firstTo", c => c.DateTime(nullable: false));
            AddColumn("dbo.doctors", "firstFrom", c => c.DateTime(nullable: false));
            AddColumn("dbo.doctors", "TmieOfAppointment", c => c.Int(nullable: false));
            DropForeignKey("dbo.doctors", "DoctorAppointments_DoctorAppointmentId", "dbo.DoctorAppointments");
            DropForeignKey("dbo.doctors", "address_AddressId", "dbo.Addresses");
            DropIndex("dbo.doctors", new[] { "DoctorAppointments_DoctorAppointmentId" });
            DropIndex("dbo.doctors", new[] { "address_AddressId" });
            DropColumn("dbo.doctors", "DoctorAppointments_DoctorAppointmentId");
            DropColumn("dbo.doctors", "address_AddressId");
            DropTable("dbo.DoctorAppointments");
            DropTable("dbo.Addresses");
        }
    }
}
