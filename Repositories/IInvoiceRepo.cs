using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface IInvoiceRepo
    {
        public List<Invoice>? GetAllCustomerInvoices(string CustomerId);
        public Invoice? GetInvoiceById(int IID);
        public int CreateInvoice(Invoice CInvoice);
        public bool EditInvoice(int IID, Invoice EInvoice);
        public bool DeleteInvoice(int IID);
    }
}
