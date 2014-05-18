namespace Erbsenzaehler.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Models.Db>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            //ContextKey = "Erbsenzaehler.Models.Db";
        }

        protected override void Seed(Models.Db context)
        {

        }
    }
}
