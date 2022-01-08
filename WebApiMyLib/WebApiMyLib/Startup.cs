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
using WebApiMyLib.Data.Repositories;
using WebApiMyLib.BLL.Interfaces;
using WebApiMyLib.BLL.Services;
using WebApiMyLib.Filters;

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
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<BookDbContext>(options => 
            options.UseSqlServer(connectionString, b => b.MigrationsAssembly("WebApiMyLib")));
            services.AddTransient<IBookRepository, BookRepository>();
            services.AddTransient<IAuthorRepository, AuthorRepository>();
            services.AddTransient<IAuthorService, AuthorService>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IBookService, BookService>();
            services.AddTransient<IValidationService<Category>, CategoryValidationService>();
            services.AddTransient<IValidationService<Author>, AuthorValidationService >();
            services.AddTransient<IValidationService<Book>, BookValidationService>();
            services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddMvc(options => options.Filters.Add(new ExceptionFilter()));
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
