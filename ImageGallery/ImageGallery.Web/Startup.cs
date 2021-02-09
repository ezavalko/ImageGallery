using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using ImageGallery.Core.Data;
using ImageGallery.Core.Repository.Base;
using ImageGallery.Core.Api;
using ImageGallery.Core.Repository.Interfaces;
using ImageGallery.Core.Repository.Implementations;
using ImageGallery.Core.Models;
using ImageGallery.Core.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace ImageGallery.Web
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
            services.AddTransient(typeof(IRepository<>), typeof(EFRespository<>));
            services.AddTransient<IImageDetailsRepository, ImageDetailsRepository>();
            services.AddTransient<IAPIImageClient, APIImageClient>();

            services.AddControllersWithViews();
            services.Configure<AppSettings>(Configuration.GetSection("ImageGallerySettings"));

            var updateIntervalInMinutes = Configuration.GetSection("CacheOptions").GetValue(typeof(int), "UpdateIntervalInMinutes");

            services.AddDbContext<ImageGalleryContext>(opt => opt.UseInMemoryDatabase(databaseName: "InMemoryDb"), ServiceLifetime.Scoped);
            
            services.AddHostedService<CacheUpdateService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IAPIImageClient apiClient)
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
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{searchTerm?}");
            });
        }
    }
}
