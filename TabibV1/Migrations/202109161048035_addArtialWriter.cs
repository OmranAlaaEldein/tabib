namespace TabibV1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addArtialWriter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.articals", "articlWriter", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.articals", "articlWriter");
        }
    }
}
