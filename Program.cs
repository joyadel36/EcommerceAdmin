using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //Localization
            builder.Services.AddLocalization(op=>op.ResourcesPath= "Resources");
            builder.Services.AddMvc()
               .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
               .AddDataAnnotationsLocalization();

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("ar") 
                };

                options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            //DataBase
            builder.Services.AddDbContext<ECommerceContext>(
            op => op.UseSqlServer(builder.Configuration.GetConnectionString("myConn")));
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
                op =>
                {
                    op.Password.RequireLowercase = true;
                    op.Password.RequiredLength = 10;
                    op.Password.RequireUppercase = true;
                    op.Password.RequiredUniqueChars = 1;
                    op.Password.RequireDigit = true;
                    op.User.RequireUniqueEmail = true;

                })
                .AddEntityFrameworkStores<ECommerceContext>();

            //Cookie Time 
            builder.Services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromHours(3);
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.SlidingExpiration = false;

            });

            //session
            builder.Services.AddSession();
            builder.Services.AddDistributedMemoryCache();

            //Services Injection
            builder.Services.AddScoped<ICategoryItemRepo,CategoryItemServices >();
            builder.Services.AddScoped<ICategoryRepo, CategoryServices>();
            builder.Services.AddScoped<IInvoiceItemRepo, InvoiceItemServices>();
            builder.Services.AddScoped<IInvoiceRepo, InvoiceServices>();
            builder.Services.AddScoped<ISessionRepo, SessionServices>();
            builder.Services.AddScoped<ICustomersRepo, CustomersServices>();

          

            //builder.Services.AddSession();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseRequestLocalization();
          
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();


           

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
