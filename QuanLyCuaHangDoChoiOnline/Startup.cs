using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;


namespace QuanLyCuaHangDoChoiOnline
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(option =>
                {
                    option.LoginPath = new PathString("/Account/Login");
                    option.LogoutPath = new PathString("/Account/Logout");
                    option.ExpireTimeSpan = TimeSpan.FromMinutes(120);

                });
            services.AddAuthorization();
            services.AddHttpContextAccessor();

        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "search",
                pattern: "Toy/ToyName",
                defaults: new { controller = "Toy", action = "ToyName" }
                );
                endpoints.MapControllerRoute(
                    name: "category1",
                    pattern: "Toy/{page?}",
                    defaults: new { controller = "Toy", action = "Index" }
                );
                endpoints.MapControllerRoute(
                    name: "category",
                    pattern: "Toy/ToyType/{ID}/{page?}",
                    defaults: new { controller = "Toy", action = "ToyType" }
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
