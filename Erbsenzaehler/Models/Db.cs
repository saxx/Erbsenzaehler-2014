using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Erbsenzaehler.Models
{
    public class Db : IdentityDbContext<User>
    {
        public Db()
            : base("Db")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Line>()
                .HasOptional(x => x.Category)
                .WithMany(x => x.Lines)
                .HasForeignKey(x => x.CategoryId);
            modelBuilder.Entity<Line>()
                .HasRequired(x => x.Client)
                .WithMany(x => x.Lines)
                .HasForeignKey(x => x.ClientId)
                .WillCascadeOnDelete(true);
            modelBuilder.Entity<Line>()
                .HasRequired(x => x.Account)
                .WithMany(x => x.Lines)
                .HasForeignKey(x => x.AccountId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Category>()
                .HasRequired(x => x.Client)
                .WithMany(x => x.Categories)
                .HasForeignKey(x => x.ClientId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Account>()
                .HasRequired(x => x.Client)
                .WithMany(x => x.Accounts)
                .HasForeignKey(x => x.ClientId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }

        public IDbSet<Client> Clients { get; set; }
        public IDbSet<Category> Categories { get; set; }
        public IDbSet<Account> Accounts { get; set; }
        public IDbSet<Line> Lines { get; set; }
    }
}