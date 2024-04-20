using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Xml.Linq;
using WebApplication1.Models;
using WebApplication1.Repositories;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class CategoryItemsController : Controller
    {
        public readonly ICategoryItemRepo _CategoryItem;
        private readonly ICategoryRepo _Category;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private string _enResourcePathAll;
        private string _arResourcePathAll;
        private string _enResourcePathDetails;
        private string _arResourcePathDetails;
        private string _enResourcePathInvoice;
        private string _arResourcePathInvoice;
        private string _enResourcePathInvoiceItem;
        private string _arResourcePathInvoiceItem;
        public CategoryItemsController(ICategoryItemRepo categoryItem, ICategoryRepo category, IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _Category = category;
            _CategoryItem = categoryItem;

            _enResourcePathAll = Path.Combine(_hostingEnvironment.ContentRootPath, "Resources", "Views", "CategoryItems", "GetAllCategoryItems.en-us.resx");
            _arResourcePathAll = Path.Combine(_hostingEnvironment.ContentRootPath, "Resources", "Views", "CategoryItems", "GetAllCategoryItems.ar.resx");

            _enResourcePathDetails = Path.Combine(_hostingEnvironment.ContentRootPath, "Resources", "Views", "CategoryItems", "GetCategoryItemDetails.en-us.resx");
            _arResourcePathDetails = Path.Combine(_hostingEnvironment.ContentRootPath, "Resources", "Views", "CategoryItems", "GetCategoryItemDetails.ar.resx");
            
            _enResourcePathInvoice= Path.Combine(_hostingEnvironment.ContentRootPath, "Resources", "Views", "Invoice", "FinishAddingItemsTotheCustomerInvoice.en-us.resx");
            _arResourcePathInvoice=Path.Combine(_hostingEnvironment.ContentRootPath, "Resources", "Views", "Invoice", "FinishAddingItemsTotheCustomerInvoice.ar.resx");

           _enResourcePathInvoiceItem= Path.Combine(_hostingEnvironment.ContentRootPath, "Resources", "Views", "InvoiceItem", "CreateNewInvoiceItem.en-us.resx");
           _arResourcePathInvoiceItem= Path.Combine(_hostingEnvironment.ContentRootPath, "Resources", "Views", "InvoiceItem", "CreateNewInvoiceItem.ar.resx");
 

        }

        //category Id
        public IActionResult GetAllCategoryItems(int Id)
        {

            List<CategoryItem>? DBCategoryItems = _CategoryItem.GetAllCategoryItemsByCategoryId(Id);

            if (DBCategoryItems != null)
            {
                List<CategoryItemViewModel> VMCategoryItems = new List<CategoryItemViewModel>();

                foreach (CategoryItem CI in DBCategoryItems)
                {
                    CategoryItemViewModel VMCategoryItem = ConvertCategoryitemDbToVmforAllItems(CI);

                    VMCategoryItems.Add(VMCategoryItem);

                }
                return View(VMCategoryItems);
            }
            return View();
        }

        //Item Id
        [HttpGet]
        public IActionResult GetCategoryItemDetails(int Id)
        {

            CategoryItem? DBcategoryItem = _CategoryItem.GetCategoryItemByID(Id);
            CategoryItemViewModel VMCategoryItem = ConvertCategoryitemDbToVmforAllItems(DBcategoryItem);

            return View(VMCategoryItem);
        }


        [HttpGet]
        public IActionResult CreateCategoryItem()
        {

            ViewBag.Catigories = LanguageOfAllCategoriesNames();
            return View(new CategoryItemViewModel());
        }

         [HttpPost]
        public IActionResult CreateCategoryItem(CategoryItemViewModel NewcategoryItem)
        {
            ViewBag.Catigories = LanguageOfAllCategoriesNames();


            if (ModelState.IsValid)
            {
                try
                {
                   //upload image 
                    if (NewcategoryItem.clientFile != null)
                    {
                        NewcategoryItem.dbImage = ConvertClientFileInToBinarryImage(NewcategoryItem.clientFile);
                    }

                    //convert vm to dbm
                    CategoryItem TempCtegoryItem = ConvertCategoryitemVmToDb(NewcategoryItem);
                    int createSuccess = _CategoryItem.CreateCategoryItem(TempCtegoryItem);
                  
                    if (createSuccess>-1)
                    {
                        //add to resource language
                        SetLocalizedValue(_enResourcePathDetails, "n"+NewcategoryItem.ItemName.Split(" ")[0].ToLower() + createSuccess, NewcategoryItem.ItemName);
                        SetLocalizedValue(_arResourcePathDetails, "n"+NewcategoryItem.ItemName.Split(" ")[0].ToLower() + createSuccess, NewcategoryItem.ArItemName);
                        SetLocalizedValue(_arResourcePathDetails, "d"+NewcategoryItem.ItemDescription.Split(" ")[0].ToLower() + createSuccess, NewcategoryItem.ArItemDescription);
                        SetLocalizedValue(_enResourcePathDetails, "d"+NewcategoryItem.ItemDescription.Split(" ")[0].ToLower() + createSuccess, NewcategoryItem.ItemDescription);
                        SetLocalizedValue(_enResourcePathAll,"n"+NewcategoryItem.ItemName.Split(" ")[0].ToLower() + createSuccess, NewcategoryItem.ItemName);
                        SetLocalizedValue(_arResourcePathAll, "n"+NewcategoryItem.ItemName.Split(" ")[0].ToLower() + createSuccess, NewcategoryItem.ArItemName);
                        SetLocalizedValue(_enResourcePathInvoice, "n" + NewcategoryItem.ItemName.Split(" ")[0].ToLower() + createSuccess, NewcategoryItem.ItemName);
                        SetLocalizedValue(_arResourcePathInvoice, "n" + NewcategoryItem.ItemName.Split(" ")[0].ToLower() + createSuccess, NewcategoryItem.ArItemName);
                        SetLocalizedValue(_enResourcePathInvoiceItem, "n" + NewcategoryItem.ItemName.Split(" ")[0].ToLower() + createSuccess, NewcategoryItem.ItemName);
                        SetLocalizedValue(_arResourcePathInvoiceItem, "n" + NewcategoryItem.ItemName.Split(" ")[0].ToLower() + createSuccess, NewcategoryItem.ArItemName);

                        return View(new CategoryItemViewModel());
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "An Error Occurred, Please try creating the category item again..!");
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
        public IActionResult EditCategoryItem(int Id)
        {
            ViewBag.Catigories = _Category.GetAllCategory();
            CategoryItem? DBcategoryItem = _CategoryItem.GetCategoryItemByID(Id);

            CategoryItemViewModel VMCategoryItem = ConvertCategoryitemDbToVm(DBcategoryItem);

            return View(VMCategoryItem);
        }
        [HttpPost]
        public IActionResult EditCategoryItem(int Id, CategoryItemViewModel EditcategoryItem)
        {
            ViewBag.Catigories = _Category.GetAllCategory();

            if (ModelState.IsValid)
            {
                try
                {
                    if (EditcategoryItem.clientFile != null)
                    {
                        EditcategoryItem.dbImage = ConvertClientFileInToBinarryImage(EditcategoryItem.clientFile);
                    }
                    CategoryItem TempCtegoryItem = ConvertCategoryitemVmToDb(EditcategoryItem);
                    bool EditSuccess = _CategoryItem.EditCategoryItem(Id, TempCtegoryItem);
                    if (EditSuccess)
                    {
                        //update resource language
                        SetLocalizedValue(_enResourcePathDetails, "n" + EditcategoryItem.ItemName.Split(" ")[0].ToLower() + EditcategoryItem.Id, EditcategoryItem.ItemName);
                        SetLocalizedValue(_arResourcePathDetails, "n" + EditcategoryItem.ItemName.Split(" ")[0].ToLower() + EditcategoryItem.Id, EditcategoryItem.ArItemName);
                        SetLocalizedValue(_arResourcePathDetails, "d" + EditcategoryItem.ItemDescription.Split(" ")[0].ToLower() + EditcategoryItem.Id, EditcategoryItem.ArItemDescription);
                        SetLocalizedValue(_enResourcePathDetails, "d" + EditcategoryItem.ItemDescription.Split(" ")[0].ToLower() + EditcategoryItem.Id, EditcategoryItem.ItemDescription);
                        SetLocalizedValue(_enResourcePathAll, "n"+EditcategoryItem.ItemName.Split(" ")[0].ToLower() + EditcategoryItem.Id, EditcategoryItem.ItemName);
                        SetLocalizedValue(_arResourcePathAll, "n"+EditcategoryItem.ItemName.Split(" ")[0].ToLower() + EditcategoryItem.Id, EditcategoryItem.ArItemName);
                        SetLocalizedValue(_enResourcePathInvoice, "n" + EditcategoryItem.ItemName.Split(" ")[0].ToLower() + EditcategoryItem.Id, EditcategoryItem.ItemName);
                        SetLocalizedValue(_arResourcePathInvoice, "n" + EditcategoryItem.ItemName.Split(" ")[0].ToLower() + EditcategoryItem.Id, EditcategoryItem.ArItemName);
                        SetLocalizedValue(_enResourcePathInvoiceItem, "n" + EditcategoryItem.ItemName.Split(" ")[0].ToLower() + EditcategoryItem.Id, EditcategoryItem.ItemName);
                        SetLocalizedValue(_arResourcePathInvoiceItem, "n" + EditcategoryItem.ItemName.Split(" ")[0].ToLower() + EditcategoryItem.Id, EditcategoryItem.ArItemName);

                        return RedirectToAction("GetAllCategories", "Categories");
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "An Error Occurred, Please try Editing the category item again..!");
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
        public IActionResult DeleteCategoryItem(int Id)
        {

            bool DeleteSuccess = _CategoryItem.DeleteCategoryItem(Id);
            if (DeleteSuccess)
            {
                return RedirectToAction("GetAllCategories", "Categories");
            }
            else
            {
                ModelState.AddModelError("Error", "An Error Occurred, Please try Deleting the category again..!");
                return RedirectToAction("GetAllCategories", "Categories");
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

        private CategoryItemViewModel ConvertCategoryitemDbToVm(CategoryItem DBcategoryItem)
        {
            CategoryItemViewModel VMCategoryItem = new CategoryItemViewModel();
            VMCategoryItem.CategoryId = DBcategoryItem.CategoryId;
            VMCategoryItem.CategoryName = DBcategoryItem.categories.CategoryName;
            VMCategoryItem.ItemName = GetValue(_enResourcePathDetails, "n"+DBcategoryItem.ItemName + DBcategoryItem.Id);
            VMCategoryItem.ArItemName = GetValue(_arResourcePathDetails, "n"+DBcategoryItem.ItemName+ DBcategoryItem.Id);
            VMCategoryItem.Id = DBcategoryItem.Id;
            VMCategoryItem.ItemDescription = GetValue(_enResourcePathDetails, "d"+DBcategoryItem.ItemDescription+ DBcategoryItem.Id);
            VMCategoryItem.ArItemDescription = GetValue(_arResourcePathDetails, "d"+DBcategoryItem.ItemDescription+ DBcategoryItem.Id);
            VMCategoryItem.dbImage = DBcategoryItem.Image;
            VMCategoryItem.Price = DBcategoryItem.Price;
            VMCategoryItem.IsAvailable = DBcategoryItem.IsAvailable;
            VMCategoryItem.Discount = DBcategoryItem.Discount ?? 0;
            return VMCategoryItem;
        }

        private CategoryItemViewModel ConvertCategoryitemDbToVmforAllItems(CategoryItem DBcategoryItem)
        {
            CategoryItemViewModel VMCategoryItem = new CategoryItemViewModel();
            VMCategoryItem.CategoryId = DBcategoryItem.CategoryId;
            VMCategoryItem.CategoryName = DBcategoryItem.categories.CategoryName;
            VMCategoryItem.ItemName = DBcategoryItem.ItemName;
            VMCategoryItem.Id = DBcategoryItem.Id;
            VMCategoryItem.ItemDescription = DBcategoryItem.ItemDescription;
           VMCategoryItem.dbImage = DBcategoryItem.Image;
            VMCategoryItem.Price = DBcategoryItem.Price;
            VMCategoryItem.IsAvailable = DBcategoryItem.IsAvailable;
            VMCategoryItem.Discount = DBcategoryItem.Discount ?? 0;
            return VMCategoryItem;
        }

        private CategoryItem ConvertCategoryitemVmToDb(CategoryItemViewModel VMCategoryItem)
        {
            CategoryItem DBcategoryItem = new CategoryItem();
            DBcategoryItem.CategoryId = VMCategoryItem.CategoryId;
            DBcategoryItem.ItemName = VMCategoryItem.ItemName.Split(" ")[0].ToLower();
            DBcategoryItem.ItemDescription = VMCategoryItem.ItemDescription.Split(" ")[0].ToLower();
            DBcategoryItem.Image = VMCategoryItem.dbImage;
            DBcategoryItem.Price = VMCategoryItem.Price;
            DBcategoryItem.IsAvailable = VMCategoryItem.IsAvailable;
            DBcategoryItem.Discount = VMCategoryItem.Discount;
            return DBcategoryItem;

        }

        private byte[] ConvertClientFileInToBinarryImage(IFormFile clientFile)
        {
            MemoryStream stream = new MemoryStream();
            clientFile.CopyTo(stream);
            return stream.ToArray();
        }

        private List<Category>? LanguageOfAllCategoriesNames()
        {
            List<Category>? catigories = _Category.GetAllCategory();
            if (!catigories.IsNullOrEmpty())
            {
                string enResourcePath = Path.Combine(_hostingEnvironment.ContentRootPath, "Resources", "Views", "Categories", "GetAllCategories.en-us.resx");
                string arResourcePath = Path.Combine(_hostingEnvironment.ContentRootPath, "Resources", "Views", "Categories", "GetAllCategories.ar.resx");
                string lang = CultureInfo.CurrentCulture.Name;

                if (lang == "ar")
                {
                    foreach (Category c in catigories)
                    {
                        c.CategoryName = GetValue(arResourcePath, "n"+c.CategoryName);
                    }
                }
                else
                {
                    foreach (Category c in catigories)
                    {
                        c.CategoryName = GetValue(enResourcePath, "n"+c.CategoryName);
                    }
                }
            }
            return catigories;
        }

    }
}
