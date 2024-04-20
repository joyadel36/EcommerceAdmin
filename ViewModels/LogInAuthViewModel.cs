using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels
{
    public class LogInAuthViewModel
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "you must entr your user name.....")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "you must entr your user password.....")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
      
    }
}
