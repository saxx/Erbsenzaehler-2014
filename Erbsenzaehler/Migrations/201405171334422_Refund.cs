namespace Erbsenzaehler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Refund : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lines", "Refund", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Lines", "Refund");
        }
    }
}
