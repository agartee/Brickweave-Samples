using System;
using System.Globalization;
using System.Linq;
using BasicCqrs.Domain.People.Models;
using BasicCqrs.Domain.People.Queries;
using BasicCqrs.Domain.People.Services;
using BasicCqrs.WebApp.Converters;
using BasicCqrs.WebApp.Formatters;
using Brickweave.Cqrs.Cli.DependencyInjection;
using Brickweave.Cqrs.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BasicCqrs.WebApp
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
            services.AddControllersWithViews(config =>
                {
                    // Add the PlainTextInputFormatter to use with the CLI-enabled endpoint, as the example
                    // PowerShell client (/scripts/cli-client-nosecurity.ps1) sends web requests as "text/plain".
                    config.InputFormatters.Add(new StringInputFormatter());
                })
                .AddJsonOptions(config =>
                {
                    config.JsonSerializerOptions.WriteIndented = true;

                    // Use a System.Text.Json coverter to write Id (e.g. PersonId) value objects as their root values. 
                    // This will really only be used by the CLI in this demo, we the web application does this 
                    // conversion in the view models for Razor page consumption.
                    config.JsonSerializerOptions.Converters.Add(new IdConverter<PersonId>());
                });

            // Wire-up Brickweave library services
            var domainAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName.Contains("BasicCqrs.Domain"))
                .ToArray();
            services.AddCqrs(domainAssemblies);
            services.AddCli(domainAssemblies)
                .AddDateParsingCulture(new CultureInfo("en-US"))
                .AddCategoryHelpFile("cli-categories.json") // the file containing help documentation for domain model "categories"
                .OverrideQueryName<ListPeople>("list", "person"); // override the auto-discovered CLI command named "people list" to "person list"

            // Register your application's domain services
            services.AddSingleton<IPersonRepository, DummyPersonRepository>();
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
