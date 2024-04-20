using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace WebApplication1.Models
{
    public class ECommerceContext:IdentityDbContext<ApplicationUser>
    {

        public ECommerceContext(DbContextOptions<ECommerceContext> option) : base(option) { }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<CategoryItem> CategoryItem { get; set; }
        public virtual DbSet<Invoice> Invoice { get; set; }
        public virtual DbSet<InvoiceItem> InvoiceIteam { get; set; }
        public virtual DbSet<Sessions> Sessions { get; set; }
        public virtual DbSet<Customers> Customers { get; set; }
       }
}
