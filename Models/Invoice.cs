using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }
        public double? TotalPriceWithoutDiscount { get; set; }
        public int? Discount { get; set; }
        public double? TotalPriceAfterDiscount { get; set; }

        [ForeignKey("cusromer")]
        public int CustomerId { get; set; }
        public virtual Customers customer { get; set; }

        [ForeignKey("applicationUser")]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser applicationUser { get; set; }

        public virtual ICollection<InvoiceItem>? invoiceItems { get; set; }
    }
}
