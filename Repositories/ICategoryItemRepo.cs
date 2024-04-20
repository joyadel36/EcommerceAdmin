using WebApplication1.Models;
namespace WebApplication1.Repositories
{
    public interface ICategoryItemRepo
    {
        public List<CategoryItem>? GetAllCategoryItemsByCategoryId(int CIID);
        public CategoryItem? GetCategoryItemByID(int CIID);
        public int CreateCategoryItem(CategoryItem CCategoryItem);
        public bool EditCategoryItem(int CIID, CategoryItem EcategoryItem);
        public bool DeleteCategoryItem(int CIID);
    }
}
