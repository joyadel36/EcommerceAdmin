using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface ICategoryRepo
    {
        public List<Category>? GetAllCategory();
        public Category? GetCategoryByID(int CID);
        public bool CreateCategory(Category CCategory);
        public bool EditCategory(int CID, Category Ecategory);
        public bool DeleteCategory(int CID);
    }
}
