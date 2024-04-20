using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class InvoiceServices : IInvoiceRepo
    {
        public ECommerceContext _Context { get; set; }
        public InvoiceServices(ECommerceContext context)
        {
            _Context = context;
        }
        public int CreateInvoice(Invoice CInvoice)
        {

            if (CInvoice != null)
            {
                _Context.Invoice.Add(CInvoice);
                _Context.SaveChanges();
             
                return CInvoice.Id;

            }
            return -1;
        }

        public bool DeleteInvoice(int IID)
        {
            Invoice? tempInvoice = _Context.Invoice.Where(i => i.Id == IID).FirstOrDefault();
            if (tempInvoice != null)
            {
                _Context.Invoice.Remove(tempInvoice);
                _Context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool EditInvoice(int IID, Invoice EInvoice)
        {
            Invoice? tempInvoice = _Context.Invoice.Where(i => i.Id == IID).FirstOrDefault();
            if (tempInvoice != null)
            {
                tempInvoice.Discount= EInvoice.Discount;
                tempInvoice.TotalPriceAfterDiscount = EInvoice.TotalPriceAfterDiscount;
                tempInvoice.TotalPriceWithoutDiscount = EInvoice.TotalPriceWithoutDiscount;
                _Context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<Invoice>? GetAllCustomerInvoices(string CustomerId)
        {
            return _Context.Invoice.Where(c => c.ApplicationUserId == CustomerId).ToList();
        }

        public Invoice? GetInvoiceById(int IID)
        {
            return _Context.Invoice.Include(i=>i.applicationUser).Include(i=>i.customer).Where(i => i.Id == IID).FirstOrDefault();
        }
    }
}
