using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpensAce.Models
{
    public class Transaction
    {
        [Key]
        public Int32 TransactionId { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Provide category")]
        public Int32 CategoryId { get; set; }
        public Category? Category { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Provide amount")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public Int32 Amount { get; set; }

        [Column(TypeName = "nvarchar(75)")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Provide note")]
        public String Note { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        [NotMapped]
        public String? CategoryTitleWithIcon
        {
            get
            {
                return Category == null ? "" : $"{Category.TitleWithIcon}";
            }
        }

        [NotMapped]
        public String? AmountCategorized
        {
            get
            {
                return Category == null ? "" : $"{(Category.Type == "Expense" ? "-" : "+")}{Amount.ToString("C0")}";
            }
        }
    }
}
