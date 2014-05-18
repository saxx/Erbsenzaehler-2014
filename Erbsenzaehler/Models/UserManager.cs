using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;

namespace Erbsenzaehler.Models
{
    public class UserManager : UserManager<User>
    {
        private readonly Db _db;

        public UserManager(Db db) : base(new UserStore<User>(db))
        {
            _db = db;
        }

        public async override Task<IdentityResult> CreateAsync(User user)
        {
            var client = await BeforeCreate(user);
            var result = await base.CreateAsync(user);
            return await AfterCreate(result, client);
        }

        private async Task<Client> BeforeCreate(User user)
        {
            var client = new Client
            {
                Name = "Client for " + user.UserName
            };
            _db.Clients.Add(client);
            await _db.SaveChangesAsync();

            user.ClientId = client.Id;
            return client;
        }

        private async Task<IdentityResult> AfterCreate(IdentityResult result, Client client)
        {
            if (result.Succeeded)
            {
                _db.Accounts.Add(new Account
                {
                    ClientId = client.Id,
                    Name = "My Account"
                });
                _db.Categories.Add(new Category
                {
                    ClientId = client.Id,
                    Name = "Going out"
                });
                _db.Categories.Add(new Category
                {
                    ClientId = client.Id,
                    Name = "Food"
                });
                _db.Categories.Add(new Category
                {
                    ClientId = client.Id,
                    Name = "Travelling"
                });
                await _db.SaveChangesAsync();
            }
            else
            {
                _db.Clients.Remove(client);
                await _db.SaveChangesAsync();
            }

            return result;
        }
    }
}