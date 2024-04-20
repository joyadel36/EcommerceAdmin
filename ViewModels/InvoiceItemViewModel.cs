
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace WebApplication1.ViewModels
{
    [PrimaryKey("InvoiceId", "ItemId")]
    public class InvoiceItemViewModel
    {
        [Required(ErrorMessage = "you must choose item ...!!")]
        public int ItemId { get; set; }

       public string? ItemName { get; set; }
        public int? TotalPriceForTheItem { get; set; }
        public int? ItemDiscount { get; set; }

        public double? Price { get; set; }
        [Required(ErrorMessage = "you must choose Category  ...!!")]
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int? Amount { get; set; }
        public int? Discount { get; set; }
   
        public int? InvoiceId { get; set; }
    

    }
}
