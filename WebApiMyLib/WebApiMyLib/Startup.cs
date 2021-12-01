using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiMyLib.Data.Models;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using WebApiMyLib.Repository.Repositories;

namespace WebApiMyLib
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
            services.AddDbContext<BookDbContext>(options => 
            options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=MyLibrary;Trusted_Connection=True;MultipleActiveResultSets=true;"));
            services.AddTransient<IBookRepository, BookRepository>();
            services.AddTransient<IAutorRepository, AutorRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStatusCodePages();
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    null,
                    "{controller=Home}/{action=Index}");
            });
        }
    }
}
