namespace TabibV1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class consulationSpecilate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.consulations", "specialty", c => c.String(maxLength: 50));
            DropColumn("dbo.consulations", "KeyWords");
        }
        
        public override void Down()
        {
            AddColumn("dbo.consulations", "KeyWords", c => c.String(maxLength: 50));
            DropColumn("dbo.consulations", "specialty");
        }
    }
}
