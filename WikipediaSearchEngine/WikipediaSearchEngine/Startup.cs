using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nest;
using System;
using Microsoft.OpenApi.Models;
using WikipediaSearchEngine.Models;

namespace WikipediaSearchEngine
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
            services.AddControllersWithViews().AddRazorRuntimeCompilation();


            RegisterElasticsearch(services);
            RegisterSwagger(services);
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

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Wikipedia Search Engine Api");
            });
        }

        private void RegisterElasticsearch(IServiceCollection services)
        {
            var url = Configuration["Elastic:Url"];
            var defaultIndex = Configuration["Elastic:Index"];

            var settings = new ConnectionSettings(new Uri(url))
                .DefaultIndex(defaultIndex)
                .DefaultMappingFor<Document>(m => m);

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);
        }

        private static void RegisterSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Wikipedia Search Engine Api",
                    Description = "Api examples"
                });
            });
        }
    }
}