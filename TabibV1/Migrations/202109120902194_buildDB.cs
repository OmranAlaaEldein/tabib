namespace TabibV1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class buildDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.appointments",
                c => new
                    {
                        appointmentsId = c.Int(nullable: false, identity: true),
                        appointmentsTime = c.DateTime(nullable: false),
                        doctor_doctorsId = c.String(maxLength: 128),
                        patient_patientsId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.appointmentsId)
                .ForeignKey("dbo.doctors", t => t.doctor_doctorsId)
                .ForeignKey("dbo.patients", t => t.patient_patientsId)
                .Index(t => t.doctor_doctorsId)
                .Index(t => t.patient_patientsId);
            
            CreateTable(
                "dbo.doctors",
                c => new
                    {
                        doctorsId = c.String(nullable: false, maxLength: 128),
                        address = c.String(),
                        evaluate = c.Int(nullable: false),
                        specialty = c.String(),
                        TmieOfAppointment = c.Int(nullable: false),
                        firstFrom = c.DateTime(nullable: false),
                        firstTo = c.DateTime(nullable: false),
                        secondFrom = c.DateTime(nullable: false),
                        secondTo = c.DateTime(nullable: false),
                        Days = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.doctorsId)
                .ForeignKey("dbo.AspNetUsers", t => t.doctorsId)
                .Index(t => t.doctorsId);
            
            CreateTable(
                "dbo.consulations",
                c => new
                    {
                        consulationID = c.Int(nullable: false, identity: true),
                        title = c.String(nullable: false, maxLength: 50),
                        Question = c.String(nullable: false, maxLength: 500),
                        DateOfconsulation = c.DateTime(nullable: false),
                        reply = c.String(maxLength: 500),
                        Isreplaied = c.Boolean(nullable: false),
                        DateOfreply = c.DateTime(),
                        PathImage = c.String(),
                        KeyWords = c.String(maxLength: 50),
                        doctor_doctorsId = c.String(maxLength: 128),
                        patient_patientsId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.consulationID)
                .ForeignKey("dbo.doctors", t => t.doctor_doctorsId)
                .ForeignKey("dbo.patients", t => t.patient_patientsId)
                .Index(t => t.doctor_doctorsId)
                .Index(t => t.patient_patientsId);
            
            CreateTable(
                "dbo.patients",
                c => new
                    {
                        patientsId = c.String(nullable: false, maxLength: 128),
                        countVisit = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.patientsId)
                .ForeignKey("dbo.AspNetUsers", t => t.patientsId)
                .Index(t => t.patientsId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        DateBirthday = c.DateTime(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.LoginProvider, t.ProviderKey })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.searches",
                c => new
                    {
                        searchesID = c.Int(nullable: false, identity: true),
                        titleOfsearches = c.String(nullable: false, maxLength: 100),
                        typeOfsearches = c.String(),
                        Textsearches = c.String(nullable: false),
                        PathOfImage = c.String(),
                        DateOfsearches = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.searchesID);
            
            CreateTable(
                "dbo.articals",
                c => new
                    {
                        articalsID = c.Int(nullable: false, identity: true),
                        titleOfarticls = c.String(nullable: false, maxLength: 100),
                        typeOfarticls = c.String(),
                        TextArticles = c.String(nullable: false),
                        PathOfImage = c.String(),
                        DateOfarticals = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.articalsID);
            
            CreateTable(
                "dbo.myModels",
                c => new
                    {
                        myModelId = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        path = c.String(),
                        evaluate = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.myModelId);
            
            CreateTable(
                "dbo.searchesdoctors",
                c => new
                    {
                        searches_searchesID = c.Int(nullable: false),
                        doctors_doctorsId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.searches_searchesID, t.doctors_doctorsId })
                .ForeignKey("dbo.searches", t => t.searches_searchesID, cascadeDelete: true)
                .ForeignKey("dbo.doctors", t => t.doctors_doctorsId, cascadeDelete: true)
                .Index(t => t.searches_searchesID)
                .Index(t => t.doctors_doctorsId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.doctors", "doctorsId", "dbo.AspNetUsers");
            DropForeignKey("dbo.searchesdoctors", "doctors_doctorsId", "dbo.doctors");
            DropForeignKey("dbo.searchesdoctors", "searches_searchesID", "dbo.searches");
            DropForeignKey("dbo.patients", "patientsId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.consulations", "patient_patientsId", "dbo.patients");
            DropForeignKey("dbo.appointments", "patient_patientsId", "dbo.patients");
            DropForeignKey("dbo.consulations", "doctor_doctorsId", "dbo.doctors");
            DropForeignKey("dbo.appointments", "doctor_doctorsId", "dbo.doctors");
            DropIndex("dbo.doctors", new[] { "doctorsId" });
            DropIndex("dbo.searchesdoctors", new[] { "doctors_doctorsId" });
            DropIndex("dbo.searchesdoctors", new[] { "searches_searchesID" });
            DropIndex("dbo.patients", new[] { "patientsId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "User_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.consulations", new[] { "patient_patientsId" });
            DropIndex("dbo.appointments", new[] { "patient_patientsId" });
            DropIndex("dbo.consulations", new[] { "doctor_doctorsId" });
            DropIndex("dbo.appointments", new[] { "doctor_doctorsId" });
            DropTable("dbo.searchesdoctors");
            DropTable("dbo.myModels");
            DropTable("dbo.articals");
            DropTable("dbo.searches");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.patients");
            DropTable("dbo.consulations");
            DropTable("dbo.doctors");
            DropTable("dbo.appointments");
        }
    }
}
