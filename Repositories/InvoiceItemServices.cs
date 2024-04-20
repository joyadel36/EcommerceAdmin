using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class InvoiceItemServices : IInvoiceItemRepo
    {
        public ECommerceContext _Context { get; set; }
        public InvoiceItemServices(ECommerceContext context)
        {
            _Context = context;
        }
        public bool CreateInvoiceItem(InvoiceItem CInvoiceItem)
        {
            try
            {
                if (CInvoiceItem != null)
                {
                    _Context.InvoiceIteam.Add(CInvoiceItem);
                    _Context.SaveChanges();
                    return true;
                }
            }
            catch
            {

                return false;
            }

            return false;

        }

        public bool DeleteInvoiceItem(int InvoiceID, int ItemID)
        {
            InvoiceItem? tempinvoiceiteam = _Context.InvoiceIteam.Where(i=>i.InvoiceId== InvoiceID && i.ItemId== ItemID).FirstOrDefault();
             if (tempinvoiceiteam != null)
            {
                _Context.InvoiceIteam.Remove(tempinvoiceiteam);
                _Context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool EditInvoiceItem(int InvoiceID, int ItemID, InvoiceItem EInvoiceItem)
        {
            InvoiceItem? tempinvoiceiteam = _Context.InvoiceIteam.Where(i => i.InvoiceId == InvoiceID && i.ItemId == ItemID).FirstOrDefault();
            if (tempinvoiceiteam != null)
            {
                tempinvoiceiteam.Discount = EInvoiceItem.Discount;
                tempinvoiceiteam.Amount = EInvoiceItem.Amount;
                tempinvoiceiteam.TotalPrice = EInvoiceItem.TotalPrice;
                _Context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<InvoiceItem>? GetAllInvoiceItems(int InvoiceID)
        {
         return _Context.InvoiceIteam.Include(i=>i.categoryItem).Where(i => i.InvoiceId == InvoiceID).ToList();
        }

        public InvoiceItem? GetInvoiceItem(int InvoiceID, int ItemID)
        {
            return  _Context.InvoiceIteam.Include(i=>i.categoryItem).Where(i => i.InvoiceId == InvoiceID && i.ItemId == ItemID).FirstOrDefault();

        }
    }
}
