using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class CategoryViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "you must entr your Category name.....")]
        public string CategoryName { set; get; }
        public string ArCategoryName { set; get; }

        //        public List<CategoryItem> categoryItems { get; set; } = new List<CategoryItem>();
    }
}
