using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface IInvoiceItemRepo
    {
        public List<InvoiceItem>? GetAllInvoiceItems(int InvoiceID);
        public InvoiceItem? GetInvoiceItem(int InvoiceID,int ItemID);
        public bool CreateInvoiceItem(InvoiceItem CInvoiceItem);
        public bool EditInvoiceItem(int InvoiceID,int ItemID, InvoiceItem EInvoiceItem);
        public bool DeleteInvoiceItem(int InvoiceID,int ItemID);
    }
}
