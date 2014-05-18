using System;
using System.ComponentModel.DataAnnotations;

namespace Erbsenzaehler.Models
{
    public class Line
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int? CategoryId { get; set; }
        public int AccountId { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime Date { get; set; }

        public DateTime OriginalDate { get; set; }
        public DateTime CreationDateUtc { get; set; }
        public DateTime? RefundDate { get; set; }

        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        public decimal Amount { get; set; }
        public bool Ignore { get; set; }

        public virtual Client Client { get; set; }
        public virtual Category Category { get; set; }
        public virtual Account Account { get; set; }
    }
}