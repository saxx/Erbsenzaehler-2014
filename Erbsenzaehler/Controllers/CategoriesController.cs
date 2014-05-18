using Erbsenzaehler.Models;
using Erbsenzaehler.Services;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Erbsenzaehler.Controllers
{
    [Authorize]
    public class CategoriesController : BaseController
    {
        public async Task<ActionResult> Index()
        {
            var currentClient = await GetCurrentClient();
            var categories = Db.Categories.Where(x => x.ClientId == currentClient.Id).OrderBy(x => x.Name);
            return View(categories.ToList());
        }

        public async Task<ActionResult> ResetAllCategories()
        {
            var service = new FindCategoryService();
            await service.ResetCategoriesForLines(await GetCurrentClient(), Db);
            return RedirectToAction("Index");
        }

        #region Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name,Regex")] Category category)
        {
            if (ModelState.IsValid)
            {
                var currentClient = await GetCurrentClient();
                category.Client = currentClient;

                Db.Categories.Add(category);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }
        #endregion

        #region Edit
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var currentClient = await GetCurrentClient();
            var category = Db.Categories.FirstOrDefault(x => x.Id == id.Value && x.ClientId == currentClient.Id);
            if (category == null)
                return HttpNotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Regex")] Category category)
        {
            if (ModelState.IsValid)
            {
                var currentClient = await GetCurrentClient();

                var categoryInDatabase = Db.Categories.Find(category.Id);
                if (categoryInDatabase.ClientId != currentClient.Id)
                    return HttpNotFound();

                categoryInDatabase.Name = category.Name;
                categoryInDatabase.Regex = category.Regex;
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }
        #endregion

        #region Delete
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var currentClient = await GetCurrentClient();
            var category = Db.Categories.FirstOrDefault(x => x.Id == id.Value && x.ClientId == currentClient.Id);
            if (category == null)
                return HttpNotFound();

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var currentClient = await GetCurrentClient();
            var category = Db.Categories.FirstOrDefault(x => x.Id == id && x.ClientId == currentClient.Id);

            Db.Categories.Remove(category);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion
    }
}
