using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebApplication1.Models
{
    public class Sessions
    {
        [Key]
        public int Id { get; set; }

        public string UserEmail { get; set; }

        public DateTime LastSessionTime { get; set; }

        [ForeignKey("applicationUser")]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser applicationUser { get; set; }


    }
}
