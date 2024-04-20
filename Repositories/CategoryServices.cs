using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class CategoryServices : ICategoryRepo
    {
        public ECommerceContext _Context { get; set; }
        public CategoryServices(ECommerceContext context)
        {
            _Context = context;
        }

        public bool CreateCategory(Category CCategory)
        {
            if (CCategory != null)
            {
                _Context.Category.Add(CCategory);
                _Context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeleteCategory(int CID)
        {
            Category? tempcategory=_Context.Category.Where(C => C.Id== CID).FirstOrDefault();
            List<CategoryItem>? tempcategoryItems = _Context.CategoryItem.Where(CI=>CI.CategoryId==CID).ToList();
            if (tempcategory != null && tempcategoryItems.IsNullOrEmpty())
            {
                _Context.Category.Remove(tempcategory);
                _Context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool EditCategory(int CID, Category Ecategory)
        {
            Category? tempcategory = _Context.Category.Where(C => C.Id == CID).FirstOrDefault();
            if (tempcategory != null)
            {
                tempcategory.CategoryName = Ecategory.CategoryName;
                _Context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<Category>? GetAllCategory()
        {
            return _Context.Category.ToList();
        }

        public Category? GetCategoryByID(int CID)
        {
            return _Context.Category.Include(item => item.categoryItems).Where(c=>c.Id==CID).FirstOrDefault();
        }
    }
}
