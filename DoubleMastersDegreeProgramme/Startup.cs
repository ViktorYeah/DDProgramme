using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using DoubleMastersDegreeProgramme.Models;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Mvc;
using DoubleMastersDegreeProgramme.Controllers;

namespace DoubleMastersDegreeProgramme
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
            // 1. add localization service
            //services.AddTransient<IStringLocalizer, CustomStringLocalizer>();
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            // 2. add link to db conf
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // 3. add users & roles
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<ApplicationContext>()
            .AddDefaultTokenProviders();

            // 4. add mvc
            services
                .AddMvc()
                .AddDataAnnotationsLocalization()
                .AddViewLocalization();

            // 5. add cultures / set up defaults
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("uk")
                };

                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            // 6. add HTTPS redirect options
            #if !DEBUG
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });
            #endif
        }

        // HTTP request pipeline conf
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // 1. conf HTTPS redirect using Rewrite options
            #if !DEBUG
            var options = new RewriteOptions()
                .AddRedirectToHttpsPermanent();
                //.AddRedirectToHttps();
            
            app.UseRewriter(options);
            #endif

            // 2. enable static files from /wwwroot dir
            app.UseStaticFiles();

            // 3. localization conf
            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            var supportedCultures = new[]
            {
                new CultureInfo("en"),
                new CultureInfo("uk")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            // 4. 
            app.UseAuthentication();

            // 5. conf mvc routes
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
