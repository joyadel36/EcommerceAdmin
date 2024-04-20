using API_FinalTask.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebApplication1.ViewModels
{
    public class CategoryItemViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "you must enter Product name ...!!")]
        [MaxLength(20, ErrorMessage = " name must be less than 20 characters"), MinLength(3, ErrorMessage = " name must be more than 3 characters")]
        public string ItemName { get; set; }

        [Required(ErrorMessage = "you must enter Product name ...!!")]
        [MaxLength(20, ErrorMessage = " name must be less than 20 characters"), MinLength(3, ErrorMessage = " name must be more than 3 characters")]
        public string ArItemName { get; set; }

        [Required(ErrorMessage = "you must enter Product description  ...!!")]
        public string ItemDescription { get; set; }

        [Required(ErrorMessage = "you must enter Product description  ...!!")]
        public string ArItemDescription { get; set; }

        [Required(ErrorMessage = "you must enter this item is available or not ...!!")]
        public bool IsAvailable { get; set; }

        public int? Discount { get; set; }

        [Required(ErrorMessage = "you must enter Product Price  ...!!")]
        [PriceValidate]
        public double Price { get; set; }

        public int CategoryId { get; set; }
        public string? CategoryName{ get; set; }
        [NotMapped]
        public IFormFile? clientFile { get; set; }

        public byte[]? dbImage { get; set; }
        [NotMapped]
        public string? imageSrc
        {
            get
            {
                if (dbImage != null)
                {
                    string base64String = Convert.ToBase64String(dbImage, 0, dbImage.Length);
                    return "data:image/jpg;base64," + base64String;
                }
                else
                {
                    return string.Empty;
                }
            }
        }



    }
}
