using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Xml.Linq;
using WebApplication1.Models;
using WebApplication1.Repositories;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepo _Category;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private string _enResourcePath;
        private string _arResourcePath;

        public CategoriesController(ICategoryRepo category, IWebHostEnvironment hostingEnvironment)
        {

            _hostingEnvironment = hostingEnvironment;
            _Category = category;
            _enResourcePath = Path.Combine(_hostingEnvironment.ContentRootPath, "Resources", "Views", "Categories", "GetAllCategories.en-us.resx");
            _arResourcePath = Path.Combine(_hostingEnvironment.ContentRootPath, "Resources", "Views", "Categories", "GetAllCategories.ar.resx");

        }

        public IActionResult GetAllCategories()
        {
            List<Category>? categories = _Category.GetAllCategory();
            if (!categories.IsNullOrEmpty())
            {
                return View(categories);
            }
            else
            {
                return RedirectToAction("CreateCategory");
            }

        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View(new CategoryViewModel());
        }
        [HttpPost]
        public IActionResult CreateCategory(CategoryViewModel Newcategory)
        {

            if (ModelState.IsValid)
            {
                try
                {

                    Category TempCtegory = new Category();
                    TempCtegory.CategoryName = Newcategory.CategoryName.Split(" ")[0].ToLower();
                    bool createSuccess = _Category.CreateCategory(TempCtegory);
                    if (createSuccess)
                    {
                        SetLocalizedValue(_enResourcePath, "n"+Newcategory.CategoryName.Split(" ")[0].ToLower(), Newcategory.CategoryName);
                        SetLocalizedValue(_arResourcePath,"n"+Newcategory.CategoryName.Split(" ")[0].ToLower(), Newcategory.ArCategoryName);

                        return RedirectToAction("GetAllCategories");
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "An Error Occurred, Please try creating the category again..!");
                        return View();
                    }

                }
                catch
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }


        [HttpGet]
        public IActionResult EditCategory(int Id)
        {
            Category category = _Category.GetCategoryByID(Id)!;
            CategoryViewModel categorymodel = new CategoryViewModel();
         
            categorymodel.Id = category.Id;
            categorymodel.ArCategoryName = GetValue(_arResourcePath, "n"+category.CategoryName);
            categorymodel.CategoryName = GetValue(_enResourcePath, "n"+category.CategoryName);
            
            return View(categorymodel);
        }
        [HttpPost]
        public IActionResult EditCategory(int Id, CategoryViewModel Editcategory)
        {

            if (ModelState.IsValid)
            {
                try
                {

                    Category TempCtegory = new Category();
                    TempCtegory.CategoryName = Editcategory.CategoryName.Split(" ")[0].ToLower();

                    bool EditSuccess = _Category.EditCategory(Id, TempCtegory);
                    if (EditSuccess)
                    {
                        SetLocalizedValue(_enResourcePath, "n"+Editcategory.CategoryName.Split(" ")[0].ToLower(), Editcategory.CategoryName);
                        SetLocalizedValue(_arResourcePath, "n"+Editcategory.CategoryName.Split(" ")[0].ToLower(), Editcategory.ArCategoryName);

                        return RedirectToAction("GetAllCategories");
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "An Error Occurred, Please try Editing the category again..!");
                        return View();
                    }

                }
                catch
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        public IActionResult DeleteCategory(int Id)
        {

            bool DeleteSuccess = _Category.DeleteCategory(Id);
            if (DeleteSuccess)
            {
                return RedirectToAction("GetAllCategories");
            }
            else
            {
                ModelState.AddModelError("Error", "An Error Occurred, Please try Deleting the category again..!");
                return RedirectToAction("GetAllCategories");
            }

        }

        private void SetLocalizedValue(string filePath, string key, string value)
        {
            var doc = XDocument.Load(filePath);
            var dataElement = doc.Root.Elements("data").FirstOrDefault(d => d.Attribute("name")?.Value == key);

            if (dataElement != null)
            {
                dataElement.Element("value").Value = value;
            }
            else
            {
                var newDataElement = new XElement("data",
                    new XAttribute("name", key),
                    new XAttribute(XNamespace.Xml + "space", "preserve"),
                    new XElement("value", value));

                doc.Root.Add(newDataElement);
            }

            doc.Save(filePath);

        }
        private string GetValue(string filePath, string key)
        {
            var doc = XDocument.Load(filePath);
            var dataElement = doc.Root.Elements("data").FirstOrDefault(d => d.Attribute("name")?.Value == key);

            if (dataElement != null)
            {
                return dataElement.Element("value")!.Value.ToString();
            }
            return "Not fount";
        }
    }
}
