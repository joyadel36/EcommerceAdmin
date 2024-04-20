using API_FinalTask.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class CategoryItem
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "you must enter Product name ...!!")]
        [MaxLength(20, ErrorMessage = " name must be less than 20 characters"), MinLength(3, ErrorMessage = " name must be more than 3 characters")]
        public string ItemName { get; set; }

        public string? ItemDescription { get; set; }

        [Required(ErrorMessage = "you must enter this item is available or not ...!!")]
        public bool IsAvailable { get; set; }

        public int? Discount { get; set; }

        [Required(ErrorMessage = "you must enter Product Price  ...!!")]
        [PriceValidate]
        public double Price { get; set; }

        public byte[]? Image { get;  set; }

        [ForeignKey("categories")]
        public  int CategoryId { get; set; }
        public virtual Category categories{ get; set; }

    }
}
