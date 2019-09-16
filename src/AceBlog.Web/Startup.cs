using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Ace;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.FileProviders;
using System.Reflection;
using System.IO;
using Microsoft.Extensions.DependencyModel;
using System.Text;
using Ace.Web.NLog;
using AceBlog.Web.Infrastructure;
using Ace.Caching;
using Ace.Caching.Memory;

namespace AceBlog.Web
{
    public class Startup
    {
        IHostingEnvironment _env;

        public Startup(IHostingEnvironment env)
        {
            this._env = env;

            var builder = new ConfigurationBuilder()
                      .SetBasePath(env.ContentRootPath)
                      .AddJsonFile(Path.Combine("configs", "appsettings.json"), optional: true, reloadOnChange: true)
                      .AddJsonFile(Path.Combine("configs", $"appsettings.{env.EnvironmentName}.json"), optional: true, reloadOnChange: true)
                      .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(this.Configuration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ICache, AceMemoryCache>();
            Repository.ServiceCollectionExtensions.AddServices(services, this.Configuration);
            Service.ServiceCollectionExtensions.AddServices(services, this.Configuration);

            services.AddSession();
            services.AddResponseCaching();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie();

            services.AddMvc(options =>
            {
            }).AddControllersAsServices();
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            /* NLog */
            NLogExtention.ConfigureNLog(env, loggerFactory, Path.Combine("configs", "nlog.config"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseMiddleware<GlobalErrorHandlingMiddleware>();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseSession();
            app.UseResponseCaching();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "area",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
