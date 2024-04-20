using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Repositories

{
    public class CategoryItemServices : ICategoryItemRepo
    {
        public ECommerceContext _Context { get; set; }
        public CategoryItemServices(ECommerceContext context)
        {
            _Context = context;
        }
        public int CreateCategoryItem(CategoryItem CCategoryItem)
        {
            if (CCategoryItem != null)
            {
                _Context.CategoryItem.Add(CCategoryItem);
                _Context.SaveChanges();
                return CCategoryItem.Id;
            }
            return -1;
        }

        public bool DeleteCategoryItem(int CIID)
        {
            CategoryItem? tempCategoryItem = _Context.CategoryItem.Where(C => C.Id == CIID).FirstOrDefault();
              if (tempCategoryItem != null )
            {
                _Context.CategoryItem.Remove(tempCategoryItem);
                _Context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool EditCategoryItem(int CIID, CategoryItem EcategoryItem)
        {
            CategoryItem? tempcategoryItem = _Context.CategoryItem.Where(C => C.Id == CIID).FirstOrDefault();
            if (tempcategoryItem != null)
            {
                tempcategoryItem.CategoryId = EcategoryItem.CategoryId;
                tempcategoryItem.ItemName = EcategoryItem.ItemName;
                tempcategoryItem.ItemDescription = EcategoryItem.ItemDescription;
                tempcategoryItem.Image = EcategoryItem.Image;
                tempcategoryItem.Price = EcategoryItem.Price;
                tempcategoryItem.IsAvailable = EcategoryItem.IsAvailable;
                tempcategoryItem.Discount = EcategoryItem.Discount;

                _Context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<CategoryItem>? GetAllCategoryItemsByCategoryId(int CIID)
        {
            return _Context.CategoryItem.Include(i=>i.categories).Where(c=>c.CategoryId==CIID).ToList();
        }

        public CategoryItem? GetCategoryItemByID(int CIID)
        {
            return _Context.CategoryItem.Include(i=>i.categories).FirstOrDefault(i => i.Id == CIID);
        }
    }
}
