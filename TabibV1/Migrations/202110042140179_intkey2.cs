namespace TabibV1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class intkey2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.doctors", "address_Id", c => c.Int());
            AddColumn("dbo.doctors", "DoctorAppointments_Id", c => c.Int());
            CreateIndex("dbo.doctors", "address_Id");
            CreateIndex("dbo.doctors", "DoctorAppointments_Id");
            AddForeignKey("dbo.doctors", "address_Id", "dbo.Addresses", "Id");
            AddForeignKey("dbo.doctors", "DoctorAppointments_Id", "dbo.DoctorAppointments", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.doctors", "DoctorAppointments_Id", "dbo.DoctorAppointments");
            DropForeignKey("dbo.doctors", "address_Id", "dbo.Addresses");
            DropIndex("dbo.doctors", new[] { "DoctorAppointments_Id" });
            DropIndex("dbo.doctors", new[] { "address_Id" });
            DropColumn("dbo.doctors", "DoctorAppointments_Id");
            DropColumn("dbo.doctors", "address_Id");
        }
    }
}
