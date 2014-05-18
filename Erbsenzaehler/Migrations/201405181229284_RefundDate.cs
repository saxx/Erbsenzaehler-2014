namespace Erbsenzaehler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RefundDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lines", "RefundDate", c => c.DateTime());
            DropColumn("dbo.Lines", "Refund");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Lines", "Refund", c => c.Boolean(nullable: false));
            DropColumn("dbo.Lines", "RefundDate");
        }
    }
}
