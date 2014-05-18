using Microsoft.AspNet.Identity;
using Erbsenzaehler.Models;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Erbsenzaehler.Controllers
{
    public class BaseController : Controller
    {
        protected Db Db { get; private set; }
        protected UserManager<User> UserManager { get; private set; }

        public BaseController()
        {
            Db = new Db();
            UserManager = new UserManager(Db);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
                Db.Dispose();
            }
            base.Dispose(disposing);
        }

        public async Task<Client> GetCurrentClient()
        {
            var currentUser = await GetCurrentUser();
            if (currentUser != null)
                return currentUser.Client;
            return null;
        }

        public async Task<User> GetCurrentUser()
        {
            if (User.Identity.IsAuthenticated)
                return await UserManager.FindByIdAsync(User.Identity.GetUserId());
            return null;
        }
    }
}