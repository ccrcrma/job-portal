using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using job_portal.Data;
using job_portal.Extensions;
using job_portal.Services;
using job_portal.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapAreaControllerRoute(
                    name: "post",
                    areaName: "Administration",
                    pattern: "post/{action=Index}/{id?}",
                    defaults: new {controller = "Post"});

                endpoints.MapAreaControllerRoute(
                    name: "testimonial",
                    areaName: "Administration",
                    pattern: "testimonial/{action=Index}/{id?}",
                    defaults: new {controller = "Testimonial"});

                endpoints.MapAreaControllerRoute(
                    name: "dashboard",
                    areaName: "Administration",
                    pattern: "dashboard/{action=Index}/{id?}",
                    defaults: new {controller = "Dashboard"});

                
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Home}/{id?}");

            });

        }
    }
}
