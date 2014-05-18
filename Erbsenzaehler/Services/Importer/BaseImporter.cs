using CsvHelper;
using System.IO;

namespace Erbsenzaehler.Services.Importer
{
    public class BaseImporter : CsvReader
    {
        private readonly StreamReader _reader;

        protected BaseImporter(StreamReader reader)
            : base(reader)
        {
            _reader = reader;
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                _reader.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}