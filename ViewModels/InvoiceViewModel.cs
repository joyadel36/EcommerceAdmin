using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class InvoiceViewModel
    {
       
        public int Id { get; set; }
        public double? TotalPriceWithoutDiscount { get; set; }
        public int? Discount { get; set; }
        public double? TotalPriceAfterDiscount { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string ApplicationUserId { get; set; }
        public string ?ApplicationUserName { get; set; }
        public List<InvoiceItem>? invoiceIteams { get; set; } = new List<InvoiceItem>();
    }
}
