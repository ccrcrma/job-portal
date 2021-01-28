using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using job_portal.Areas.Identity.Models;
using job_portal.Data;
using job_portal.Extensions;
using job_portal.Services;
using job_portal.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace job_portal
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<ApplicationContext>(
                dbContextOptions => dbContextOptions
                    .UseMySql(
                        Configuration.GetSection("Database")["ConnectionString"],
                        new MySqlServerVersion(new Version(
                            int.Parse(Configuration["Database:DbVersion:Major"]),
                            int.Parse(Configuration["Database:DbVersion:Minor"]),
                            int.Parse(Configuration["Database:DbVersion:Build"])
                            )),
                        mySqlOptions => mySqlOptions
                            .CharSetBehavior(CharSetBehavior.NeverAppend))
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()

            );

            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationContext>()
                    .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 5;
                options.SignIn.RequireConfirmedEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/";
            });

            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddSingleton<IMailService, MailService>();
            Task.Run(() => new SeedData(services.BuildServiceProvider()).SeedAll());
            services.AddSingleton<IFileStorageService, FileStorageService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
            app.UseStatusCodePages(new StatusCodePagesOptions()
            {
                HandleAsync = (ctx) =>
                {
                    if (ctx.HttpContext.Response.StatusCode == 404)
                    {
                        var logger = app.ApplicationServices.GetService<ILogger<Startup>>();
                        logger.LogInformation(" 404 error occured");
                    }
                    return Task.FromResult(0);
                }
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRequestLoggingMiddleware();

            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapAreaControllerRoute(
                    name: "post",
                    areaName: "Administration",
                    pattern: "post/{action=Index}/{id?}",
                    defaults: new { controller = "Post" });

                endpoints.MapAreaControllerRoute(
                    name: "testimonial",
                    areaName: "Administration",
                    pattern: "testimonial/{action=Index}/{id?}",
                    defaults: new { controller = "Testimonial" });

                endpoints.MapAreaControllerRoute(
                    name: "dashboard",
                    areaName: "Administration",
                    pattern: "dashboard/{action=Index}/{id?}",
                    defaults: new { controller = "Dashboard" });

                endpoints.MapAreaControllerRoute(
                    name: "seeker-regisration",
                    areaName: "Identity",
                    pattern: "register",
                    defaults: new { controller = "Account", action = "RegisterSeekerUser" }
                );

                endpoints.MapAreaControllerRoute(
                    name: "employer-registration",
                    areaName: "Identity",
                    pattern: "employer/register",
                    defaults: new { controller = "Account", action = "RegisterEmployerUser" }
                );
                endpoints.MapAreaControllerRoute(
                    name: "identity-routes",
                    areaName: "Identity",
                    pattern: "account/{action}/{id?}",
                    defaults: new { controller = "Account" });

                endpoints.MapAreaControllerRoute(
                    name: "seeker-routes",
                    areaName: "Seeker",
                    pattern: "user/{action=Index}/{id?}",
                    defaults: new { controller = "Seeker" }
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Home}/{id?}");

            });

        }
    }
}
