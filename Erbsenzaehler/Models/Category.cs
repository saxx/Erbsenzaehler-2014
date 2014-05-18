using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Erbsenzaehler.Models
{
    public class Category
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Regex { get; set; }

        public virtual Client Client { get; set; }
        public virtual ICollection<Line> Lines { get; set; }
    }
}