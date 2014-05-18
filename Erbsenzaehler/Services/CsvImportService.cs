using CsvHelper;
using Erbsenzaehler.Models;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Erbsenzaehler.Services.Importer;

namespace Erbsenzaehler.Services
{
    public class CsvImportService
    {
        public async Task<int> RunImportAndSaveLinesToDatabase(Client client, string filePath, Account account, Db db, Importer importer)
        {
            var count = 0;
            using (var csv = BuildReader(filePath, importer))
            {
                var lines = csv.GetRecords<Line>();

                var categoryService = new FindCategoryService();
                var allCategories = await db.Categories.Where(x => x.ClientId == client.Id && x.Regex != null && x.Regex.Length > 0).ToListAsync();

                foreach (var line in lines)
                {
                    var lineExists = db.Lines.Any(x => x.ClientId == client.Id && x.OriginalDate == line.OriginalDate && x.Text == line.Text && x.Amount == line.Amount && x.AccountId == account.Id);
                    if (!lineExists)
                    {
                        line.Client = client;
                        line.Account = account;
                        line.CreationDateUtc = DateTime.UtcNow;
                        categoryService.FindCategoryForLine(line, client, db, allCategories);

                        db.Lines.Add(line);
                        await db.SaveChangesAsync();
                        count++;
                    }
                }
            }

            return count;
        }

        private CsvReader BuildReader(string filePath, Importer importer)
        {
            switch (importer)
            {
                case Importer.Easybank:
                    return new EasybankImporter(new StreamReader(filePath, Encoding.Default));
                default:
                    return new TsvImporter(new StreamReader(filePath, Encoding.UTF8));
            }
        }

        public enum Importer
        {
            Easybank,
            Tsv
        }
    }
}