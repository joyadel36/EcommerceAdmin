using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? Street { get; set; }
        public string? BuildingNumber { get; set; }

    }
}
