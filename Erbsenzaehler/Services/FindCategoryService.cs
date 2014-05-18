using System.Collections;
using System.Collections.Generic;
using Erbsenzaehler.Models;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Erbsenzaehler.Services
{
    public class FindCategoryService
    {
        public async Task<int> ResetCategoriesForLines(Client client, Db db)
        {
            var allCategories = await db.Categories.Where(x => x.ClientId == client.Id && x.Regex != null && x.Regex.Length > 0).ToListAsync();

            var changed = 0;
            foreach (var line in db.Lines.Where(x => x.ClientId == client.Id))
            {
                var categoryFound = FindCategoryForLine(line, client, db, allCategories);
                if (categoryFound)
                    changed++;
            }
            db.SaveChanges();

            return changed;
        }

        public bool FindCategoryForLine(Line line, Client client, Db db, IList<Category> allCategories)
        {
            foreach (var category in allCategories)
            {
                var pattern = category.Regex.Split('\n').Select(x => "(" + x.Trim() + ")").Aggregate("", (seed, current) => seed + "|" + current).Trim('|');

                if (Regex.IsMatch(line.Text, pattern, RegexOptions.IgnoreCase))
                {
                    line.Category = category;
                    line.CategoryId = category.Id;

                    return true;
                }
            }

            return false;
        }
    }
}