using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpensAce.Models
{
    public class Transaction
    {
        [Key]
        public Int32 TransactionId { get; set; }

        public Int32 CategoryId { get; set; }
        public Category Category { get; set; }

        public Int32 Amount { get; set; }

        [Column(TypeName = "nvarchar(75)")]
        public String Note { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

    }
}
