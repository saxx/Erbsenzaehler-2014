namespace Erbsenzaehler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreationDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lines", "CreationDateUtc", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Lines", "CreationDateUtc");
        }
    }
}
