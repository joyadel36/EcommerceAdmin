using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using WebApplication1.Models;
using WebApplication1.Repositories;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class InvoiceItemController : Controller
    {
        public readonly IInvoiceItemRepo _InvoiceItem;
        public readonly ICategoryItemRepo _CategoryItem;
        public readonly ICategoryRepo _Category;
        private readonly IWebHostEnvironment _hostingEnvironment;
   
        public InvoiceItemController(IInvoiceItemRepo invoiceItem, ICategoryItemRepo categoryItem, ICategoryRepo category, IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _Category = category;
            _CategoryItem = categoryItem;
            _InvoiceItem = invoiceItem;
        }

        [HttpGet]
        public IActionResult CreateNewInvoiceItem()
        {
              int invoiceid =(Int32) HttpContext.Session.GetInt32("InvoiceId");
            

            ViewBag.Catigories = LanguageOfAllCategoriesNames();
            ViewBag.Inviceitems = _InvoiceItem.GetAllInvoiceItems(invoiceid);

            return View(new InvoiceItemViewModel());
        }
        [HttpPost]
        public IActionResult CreateNewInvoiceItem(InvoiceItemViewModel invoiceitem)
        {
            ViewBag.Catigories = LanguageOfAllCategoriesNames();
            int invoiceid =(Int32) HttpContext.Session.GetInt32("InvoiceId");
             

            CategoryItem DBcategoryitem = _CategoryItem.GetCategoryItemByID(invoiceitem.ItemId);
            InvoiceItem DBinvoiceitem = new InvoiceItem();
            DBinvoiceitem.Amount = 1;
            DBinvoiceitem.Discount = 0;
            DBinvoiceitem.InvoiceId = invoiceid;
            DBinvoiceitem.ItemId = invoiceitem.ItemId;
            DBinvoiceitem.TotalPrice = DBcategoryitem.Price;
            _InvoiceItem.CreateInvoiceItem(DBinvoiceitem);
            ViewBag.Inviceitems = _InvoiceItem.GetAllInvoiceItems(invoiceid);
            return View(new InvoiceItemViewModel());
        }
  
        //item id
        public IActionResult DeleteInvoiceItem(int id)
        {
              int invoiceid =(Int32) HttpContext.Session.GetInt32("InvoiceId");
          
            _InvoiceItem.DeleteInvoiceItem(invoiceid, id);

            return RedirectToAction("CreateNewInvoiceItem");
        }
       
        //itemid
        public IActionResult AmountPlus(int id)
        {
            
             int invoiceid =(Int32) HttpContext.Session.GetInt32("InvoiceId");
            
            InvoiceItem? DBinvoiceitem = _InvoiceItem.GetInvoiceItem(invoiceid,id);
            InvoiceItem Einvoiceitem = new InvoiceItem();
            if(DBinvoiceitem !=null)
            {
                
                Einvoiceitem.Amount = DBinvoiceitem.Amount+1;
                Einvoiceitem.Discount = DBinvoiceitem.Discount;
                double totalprice = DBinvoiceitem.categoryItem.Price * (Einvoiceitem.Amount);

                if (DBinvoiceitem.Discount>0) {
                    double d =totalprice * (double)(DBinvoiceitem.Discount/100);
                    Einvoiceitem.TotalPrice = totalprice-d;
                }
                else {
                    Einvoiceitem.TotalPrice = totalprice;
                }
                _InvoiceItem.EditInvoiceItem(invoiceid,id, Einvoiceitem);
            }

            return RedirectToAction("CreateNewInvoiceItem");
        }

        public IActionResult AmountMinus(int id)
        {
              int invoiceid =(Int32) HttpContext.Session.GetInt32("InvoiceId");
            
            InvoiceItem? DBinvoiceitem = _InvoiceItem.GetInvoiceItem(invoiceid, id);
            InvoiceItem Einvoiceitem = new InvoiceItem();
            if (DBinvoiceitem != null && (DBinvoiceitem.Amount - 1) > 0)
            {

                Einvoiceitem.Amount = DBinvoiceitem.Amount - 1;
                Einvoiceitem.Discount = DBinvoiceitem.Discount;
                double totalprice = DBinvoiceitem.categoryItem.Price * (DBinvoiceitem.Amount - 1);

                if (DBinvoiceitem.Discount > 0)
                {
                    double d = totalprice * (double)(DBinvoiceitem.Discount / 100);
                    Einvoiceitem.TotalPrice = totalprice - d;
                }
                else
                {
                    Einvoiceitem.TotalPrice = totalprice;
                }
                _InvoiceItem.EditInvoiceItem(invoiceid, id, Einvoiceitem);
            }

            return RedirectToAction("CreateNewInvoiceItem");
        }
        public IActionResult DiscountPlus(int id)
        {
              int invoiceid =(Int32) HttpContext.Session.GetInt32("InvoiceId");
            
            InvoiceItem? DBinvoiceitem = _InvoiceItem.GetInvoiceItem(invoiceid, id);
            InvoiceItem Einvoiceitem = new InvoiceItem();
            if (DBinvoiceitem != null &&(DBinvoiceitem.Discount + 1)<=100)
            {

                Einvoiceitem.Amount = DBinvoiceitem.Amount ;
                Einvoiceitem.Discount = DBinvoiceitem.Discount+1;
                double totalprice = DBinvoiceitem.categoryItem.Price * (DBinvoiceitem.Amount);

                if (Einvoiceitem.Discount > 0)
                {
                    double d = CalcDiscount((int)Einvoiceitem.Discount, totalprice);
                    Einvoiceitem.TotalPrice = totalprice - d;
                }
                else
                {
                    Einvoiceitem.TotalPrice = totalprice;
                }
                _InvoiceItem.EditInvoiceItem(invoiceid, id, Einvoiceitem);
            }

            return RedirectToAction("CreateNewInvoiceItem");
        }
        public IActionResult DiscountMinus(int id)
        {
             int invoiceid =(Int32) HttpContext.Session.GetInt32("InvoiceId");
            
            InvoiceItem? DBinvoiceitem = _InvoiceItem.GetInvoiceItem(invoiceid, id);
            InvoiceItem Einvoiceitem = new InvoiceItem();
            if (DBinvoiceitem != null && (DBinvoiceitem.Discount - 1)>0)
            {

                Einvoiceitem.Amount = DBinvoiceitem.Amount;
                Einvoiceitem.Discount = DBinvoiceitem.Discount - 1;
                double totalprice = DBinvoiceitem.categoryItem.Price * (DBinvoiceitem.Amount);

                if (Einvoiceitem.Discount > 0)
                {
                    double d = CalcDiscount((int)Einvoiceitem.Discount, totalprice);
                    Einvoiceitem.TotalPrice = totalprice - d;
                }
                else
                {
                    Einvoiceitem.TotalPrice = totalprice;
                }
                _InvoiceItem.EditInvoiceItem(invoiceid, id, Einvoiceitem);
            }

            return RedirectToAction("CreateNewInvoiceItem");
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
                        c.CategoryName = GetValue(enResourcePath,"n"+c.CategoryName);
                    }
                }
            }
            return catigories;
        }
        private double CalcDiscount(int discount, double Price)
        {
            double percentage = (double)discount / 100;
            double d = Price * percentage;
            return d;

        }
        [HttpGet]
        public JsonResult LanguageOfAllCategoryItemsNames(int Catigoryid)
        {

            List<ItemForInvoiceItemListViewModel> AllItemForInvoiceItemList = new List<ItemForInvoiceItemListViewModel>();
           
           var CategoryItems = _CategoryItem.GetAllCategoryItemsByCategoryId(Catigoryid);

            if (!CategoryItems.IsNullOrEmpty())
            {
                 string enResourcePath = Path.Combine(_hostingEnvironment.ContentRootPath, "Resources", "Views", "CategoryItems", "GetAllCategoryItems.en-us.resx");
                string arResourcePath = Path.Combine(_hostingEnvironment.ContentRootPath, "Resources", "Views", "CategoryItems", "GetAllCategoryItems.ar.resx");


                string lang = CultureInfo.CurrentCulture.Name;

                if (lang == "ar")
                {
                    
                    foreach (CategoryItem c in CategoryItems)
                    {
                        ItemForInvoiceItemListViewModel item = new ItemForInvoiceItemListViewModel();

                        item.ItemId =c.Id;
                        c.ItemName = GetValue(arResourcePath, "n"+c.ItemName+c.Id);
                        item.ItemName = c.ItemName;
                        AllItemForInvoiceItemList.Add(item);
                    }
                }
                else
                {
                    foreach (CategoryItem c in CategoryItems)
                    {
                       
                        ItemForInvoiceItemListViewModel item = new ItemForInvoiceItemListViewModel();
                        item.ItemId = c.Id;
                        c.ItemName = GetValue(enResourcePath, "n"+ c.ItemName+ c.Id);
                        item.ItemName = c.ItemName;
                        AllItemForInvoiceItemList.Add(item);

                    }
                }
              
            }

            return Json(AllItemForInvoiceItemList);


        }

    }
}
