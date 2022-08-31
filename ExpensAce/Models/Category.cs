using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpensAce.Models
{
    public class Category
    {
        [Key]
        public Int32 CategoryId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Provide name")]
        public String Title { get; set; }

        [Column(TypeName = "nvarchar(5)")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Provide icon")]
        public String Icon { get; set; } = "";

        [Column(TypeName = "nvarchar(10)")]
        public String Type { get; set; } = "Expense";

        [NotMapped]
        public String? TitleWithIcon
        {
            get
            {
                return $"{Icon} {Title}";
            }
        }
    }
}
