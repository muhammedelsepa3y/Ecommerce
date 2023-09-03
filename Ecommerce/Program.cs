using Ecommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();
            builder.Services.AddDbContext<EcommerceContext>(options => options.UseSqlServer("Server=.;Database=Ecommerce;Trusted_Connection=True;Encrypt=False;"));
            builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<EcommerceContext>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            

            
            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            

            app.UseAuthentication();

            app.UseAuthorization();
            

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Product}/{action=AllProducts}");

            app.Run();
        }
    }
}