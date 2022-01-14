namespace TabibV1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAppointment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.appointments", "Question", c => c.String());
            AddColumn("dbo.appointments", "Note", c => c.String());
            AddColumn("dbo.appointments", "evaluation", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.appointments", "evaluation");
            DropColumn("dbo.appointments", "Note");
            DropColumn("dbo.appointments", "Question");
        }
    }
}
