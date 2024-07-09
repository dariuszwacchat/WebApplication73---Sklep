using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PayPalCheckoutSdk.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication58.Data;
using WebApplication58.Models;
using WebApplication58.Services;

namespace WebApplication58
{
    public class Startup
    {
        public Startup (IConfiguration configuration)
        {
            Configuration = configuration; 
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            })
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication()
                .AddCookie(cookie =>
                {
                    cookie.LoginPath = "/Account/Login";
                    cookie.AccessDeniedPath = "/Index/Home";
                });
            services.AddAuthorization();

            services.ConfigureApplicationCookie(cookie =>
            {
                cookie.LoginPath = "/Account/Login";
                cookie.AccessDeniedPath = "/Index/Home";
            });

            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });
            services.AddDistributedMemoryCache();
            services.AddHttpContextAccessor();
              
              
            services.AddMvc(); 
            services.AddControllersWithViews();

            
            services.AddTransient(typeof(UserService));
            services.AddTransient(typeof(KoszykService));
            services.AddTransient(typeof(HtmlSanitizationService));
            services.AddTransient(typeof(PayPalPayoutService));
            

            services.AddSingleton<PayPalHttpClient>((services) =>
            {
                var paypalOptions = Configuration.GetSection("PayPal").Get<PayPalOptions>();
                var environment = new SandboxEnvironment(paypalOptions.ClientId, paypalOptions.ClientSecret);
                return new PayPalHttpClient(environment);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();


            app.UseRouting();
            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {

                /*endpoints.MapControllerRoute(
                    name: "ProductsSubSubCategory",
                    pattern: "Products/{categoryName}/{subcategoryName}/{subsubcategoryName}",
                    defaults: new { controller = "Products", action = "Index" }
                );

                endpoints.MapControllerRoute(
                    name: "ProductsSubCategory",
                    pattern: "Products/{categoryName}/{subcategoryName}",
                    defaults: new { controller = "Products", action = "Index", subsubcategoryName = "DefaultSubSubCategory" }
                );

                endpoints.MapControllerRoute(
                    name: "ProductsCategory",
                    pattern: "Products/{categoryName}",
                    defaults: new { controller = "Products", action = "Index", subcategoryName = "DefaultSubCategory", subsubcategoryName = "DefaultSubSubCategory" }
                );*/



                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
