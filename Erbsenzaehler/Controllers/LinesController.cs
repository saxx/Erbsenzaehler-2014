using System;
using Erbsenzaehler.Models;
using Erbsenzaehler.ViewModels.Lines;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Erbsenzaehler.Controllers
{
    [Authorize]
    public class LinesController : BaseController
    {
        public ActionResult Index(string date)
        {
            return View();
        }

        public async Task<ActionResult> Json(string date)
        {
            int? selectedYear = null;
            int? selectedMonth = null;
            if (!string.IsNullOrEmpty(date))
            {
                selectedYear = int.Parse(date.Split('-')[0]);
                selectedMonth = int.Parse(date.Split('-')[1]);
            }

            var viewModel = await new IndexViewModel().Fill(await GetCurrentClient(), Db, selectedYear, selectedMonth);
            return new JsonResult
            {
                Data = viewModel,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpPost]
        public async Task<ActionResult> Json(IndexViewModel.Line line)
        {
            var currentClient = await GetCurrentClient();

            var lineInDatebase = Db.Lines.FirstOrDefault(x => x.Id == line.Id);
            if (lineInDatebase == null || lineInDatebase.ClientId != currentClient.Id)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            if (string.IsNullOrEmpty(line.Category))
                lineInDatebase.CategoryId = null;
            else
            {
                var category = Db.Categories.FirstOrDefault(x => x.ClientId == currentClient.Id && x.Name == line.Category);
                if (category != null)
                    lineInDatebase.CategoryId = category.Id;
            }
            lineInDatebase.Ignore = line.IsIgnored;
            if (string.IsNullOrEmpty(line.RefundDate))
                lineInDatebase.RefundDate = null;
            else
                lineInDatebase.RefundDate = DateTime.Parse(line.RefundDate);
            lineInDatebase.Date = DateTime.Parse(line.Date);
            Db.SaveChanges();

            return await Json("");
        }

        #region Edit

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var currentClient = await GetCurrentClient();
            var line = Db.Lines.FirstOrDefault(x => x.Id == id.Value && x.ClientId == currentClient.Id);
            if (line == null)
                return HttpNotFound();

            await SetViewBag();
            return View(line);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,CategoryId,Ignore,Text,Date,Amount")] Line line)
        {
            if (ModelState.IsValid)
            {
                var currentClient = await GetCurrentClient();
                var lineInDatabase = Db.Lines.Find(line.Id);
                if (lineInDatabase.ClientId != currentClient.Id)
                    return HttpNotFound();

                var category = Db.Categories.FirstOrDefault(x => x.Id == line.CategoryId && x.ClientId == currentClient.Id);
                if (category == null)
                    lineInDatabase.CategoryId = null;
                lineInDatabase.Category = category;
                lineInDatabase.Date = line.Date;
                lineInDatabase.Amount = line.Amount;
                lineInDatabase.Text = line.Text;
                lineInDatabase.Ignore = line.Ignore;
                Db.SaveChanges();
                return RedirectToAction("Index");
            }

            await SetViewBag();
            return View(line);
        }

        #endregion

        #region Delete
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var currentClient = await GetCurrentClient();
            var line = await Db.Lines.FirstOrDefaultAsync(x => x.Id == id.Value && x.ClientId == currentClient.Id);
            if (line == null)
                return HttpNotFound();

            return View(line);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var currentClient = await GetCurrentClient();
            var line = await Db.Lines.FirstOrDefaultAsync(x => x.Id == id && x.ClientId == currentClient.Id);
            if (line == null)
                return HttpNotFound();
            Db.Lines.Remove(line);
            await Db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        private async Task SetViewBag()
        {
            var currentClient = await GetCurrentClient();

            var categories = (await Db.Categories.Where(x => x.ClientId == currentClient.Id).OrderBy(x => x.Name).ToListAsync()).Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString(CultureInfo.InvariantCulture)
            }).ToList();

            categories.Insert(0, new SelectListItem
            {
                Text = "< No category >",
                Value = "0"
            });

            ViewBag.Categories = categories;
        }
    }
}