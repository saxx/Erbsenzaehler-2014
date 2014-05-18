namespace Erbsenzaehler.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class OriginalDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lines", "OriginalDate", c => c.DateTime(nullable: false));
            Sql("Update dbo.Lines SET OriginalDate = Date;");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Lines", "OriginalDate");
        }
    }
}
