using API_FinalTask.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    [PrimaryKey("InvoiceId", "ItemId")]
    public class InvoiceItem
    {
      
        public int Amount { get; set; }
        public int? Discount { get; set; }
        public double TotalPrice { get; set; }

        [ForeignKey("categoryItem")]
        public int ItemId { get; set; }
        public virtual CategoryItem categoryItem { get; set; }
   
    
        [ForeignKey("Invoice")]
        public int InvoiceId { get; set; }
        public virtual Invoice Invoice { get; set; }

    }
}
