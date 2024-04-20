using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Repositories;
using WebApplication1.ViewModels;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
namespace WebApplication1.Controllers
{
    [Authorize]
    public class InvoiceController : Controller
    {
        public readonly IInvoiceRepo _Invoice;
        public readonly IInvoiceItemRepo _InvoiceItem;
        public InvoiceController(IInvoiceRepo invoice, IInvoiceItemRepo invoiceItem)
        {
            _Invoice = invoice;
            _InvoiceItem = invoiceItem;
        }
       
        //customer id
        public IActionResult CreateInvoiceForCustomer(int id)
        {

            Invoice newinvoice = new Invoice();
            newinvoice.CustomerId = id;
             newinvoice.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int InvoiceId= _Invoice.CreateInvoice(newinvoice);
            if(InvoiceId!=-1)
            {
                //session
                HttpContext.Session.SetInt32("InvoiceId", InvoiceId);
            }

            return RedirectToAction("FinishAddingItemsTotheCustomerInvoice");
        }

        public IActionResult ComeFromInvoiceItems()
        {
             int invoiceid =(Int32) HttpContext.Session.GetInt32("InvoiceId");
            

            Invoice? invoice = _Invoice.GetInvoiceById(invoiceid);
            List<InvoiceItem>? invoiceItems = _InvoiceItem.GetAllInvoiceItems(invoiceid);
            ViewBag.Inviceitems = invoiceItems;
            if (invoice != null && !invoiceItems.IsNullOrEmpty())
            {
                double totalprice = 0;
                foreach (InvoiceItem item in invoiceItems)
                {
                    totalprice += item.TotalPrice;
                }
                invoice.TotalPriceWithoutDiscount = totalprice;
                if (invoice.Discount>0) 
                {
                    double discount = CalcDiscount((int)invoice.Discount,totalprice);
                    invoice.TotalPriceAfterDiscount =  totalprice-discount;
                }
                else {
                    invoice.TotalPriceAfterDiscount = totalprice;

                }
                _Invoice.EditInvoice(invoiceid, invoice);
            }

            return RedirectToAction("FinishAddingItemsTotheCustomerInvoice");
        }

        public IActionResult FinishAddingItemsTotheCustomerInvoice()
        {
            int invoiceid =(Int32) HttpContext.Session.GetInt32("InvoiceId");
          
            if(invoiceid>0)
            {
                Invoice invoice = _Invoice.GetInvoiceById(invoiceid);
                List<InvoiceItem>? invoiceItems = _InvoiceItem.GetAllInvoiceItems(invoiceid);
                ViewBag.Inviceitems = invoiceItems;
                InvoiceViewModel VMinvoice = ConvirtFromInvoiceDbToVm(invoice);
                return View(VMinvoice);
            }


            return View();
        }

        //invoice id
        public IActionResult EditInvoiceForCustomr(int id)
        {

            //get all invoice data 
            HttpContext.Session.SetInt32("InvoiceId", id);

            return RedirectToAction("FinishAddingItemsTotheCustomerInvoice");
        }

        [HttpPost]
        public IActionResult AddDiscountForCustomrInvoice(InvoiceViewModel invoice)
        {

             int invoiceid =(Int32) HttpContext.Session.GetInt32("InvoiceId");
           
            if (invoice != null && invoiceid>0)
            {
                Invoice editinvoice = _Invoice.GetInvoiceById(invoiceid);

                int discount = (int)invoice.Discount;
                if (discount > 0 && discount <= 100)
                {
                    double d = CalcDiscount(discount, (double)editinvoice.TotalPriceWithoutDiscount);
                    editinvoice.Discount = discount;
                    editinvoice.TotalPriceAfterDiscount = (double)editinvoice.TotalPriceWithoutDiscount-d;
                    _Invoice.EditInvoice(invoiceid, editinvoice);
                }
               
            }
            return RedirectToAction("FinishAddingItemsTotheCustomerInvoice");
        }
        //invoice id
        public IActionResult DeleteInvoice(int id)
        {
            Invoice? invoice = _Invoice.GetInvoiceById(id);
            List<InvoiceItem>? invoiceItems = _InvoiceItem.GetAllInvoiceItems(id);

            if (invoice != null && !invoiceItems.IsNullOrEmpty())
            {
                foreach (InvoiceItem item in invoiceItems)
                {
                    _InvoiceItem.DeleteInvoiceItem(id, item.ItemId);
                }
               
            }
            _Invoice.DeleteInvoice(id);
            return RedirectToAction("GetAllCustomers", "Customers");
        }

        public IActionResult ConfirmInvoiceForCustomrt()
        {

            HttpContext.Session.Clear();

            return RedirectToAction("GetAllCustomers", "Customers");

       
        }

        private InvoiceViewModel ConvirtFromInvoiceDbToVm(Invoice DbInvoice)
        {
            InvoiceViewModel VMinvoice = new InvoiceViewModel();
            VMinvoice.CustomerId = DbInvoice.CustomerId;
            VMinvoice.CustomerName = DbInvoice.customer.FirstName + " " + DbInvoice.customer.LastName;
            VMinvoice.ApplicationUserId = DbInvoice.ApplicationUserId;
            VMinvoice.ApplicationUserName = DbInvoice.applicationUser.UserName;
            VMinvoice.TotalPriceWithoutDiscount = DbInvoice.TotalPriceWithoutDiscount ?? 0;
            VMinvoice.TotalPriceAfterDiscount = DbInvoice.TotalPriceAfterDiscount ?? 0;
            VMinvoice.Discount = DbInvoice.Discount ?? 0;
            return VMinvoice;
        }

        private double CalcDiscount(int discount, double Price)
        {
            double percentage = (double)discount / 100;
            double d = Price * percentage;
            return d;

        }

    }
}
