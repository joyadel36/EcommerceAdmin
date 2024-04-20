using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string CategoryName { set; get; }
        public virtual ICollection<CategoryItem>? categoryItems { get; set; }
    }
}
