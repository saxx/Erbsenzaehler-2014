using System.Linq;
using Erbsenzaehler.Models;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Erbsenzaehler.Controllers
{
    public class AccountsController : BaseController
    {

        public async Task<ActionResult> Index()
        {
            var currentClient = await GetCurrentClient();
            return View(await Db.Accounts.Where(x => x.ClientId == currentClient.Id).ToListAsync());
        }

        #region Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name")] Account account)
        {
            if (ModelState.IsValid)
            {
                var currentClient = await GetCurrentClient();
                account.ClientId = currentClient.Id;
                Db.Accounts.Add(account);
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(account);
        }
        #endregion

        #region Create
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var currentClient = await GetCurrentClient();
            var account = await Db.Accounts.FirstOrDefaultAsync(x => x.Id == id.Value && x.ClientId == currentClient.Id);
            if (account == null)
                return HttpNotFound();

            return View(account);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] Account account)
        {
            if (ModelState.IsValid)
            {
                var currentClient = await GetCurrentClient();
                var accountFromDatabase = await Db.Accounts.FirstOrDefaultAsync(x => x.Id == account.Id && x.ClientId == currentClient.Id);
                if (accountFromDatabase == null)
                    return HttpNotFound();

                accountFromDatabase.Name = account.Name;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(account);
        }
        #endregion

        #region Delete
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var currentClient = await GetCurrentClient();
            var account = await Db.Accounts.FirstOrDefaultAsync(x => x.Id == id.Value && x.ClientId == currentClient.Id);
            if (account == null)
                return HttpNotFound();

            return View(account);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var currentClient = await GetCurrentClient();
            var account = await Db.Accounts.FirstOrDefaultAsync(x => x.Id == id && x.ClientId == currentClient.Id);
            if (account == null)
                return HttpNotFound();
            Db.Accounts.Remove(account);
            await Db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
    }
}
