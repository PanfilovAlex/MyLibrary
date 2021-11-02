using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace WebApiMyLib.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            BookDbContext bookDbContext = app.ApplicationServices.GetRequiredService<BookDbContext>();
            bookDbContext.Database.Migrate();
            if (!bookDbContext.Books.Any())
            {
                bookDbContext.Books.AddRange(
                    new Book
                    {
                        Autor = "Адам Фримен",
                        Title = "ASP.NET Core MVC 2 для профессионалов",
                        Category = ".Net"
                    },
                    new Book {
                        Autor = "Троелсон",
                        Title = "C# 7.0",
                        Category = "C#"
                    },
                    new Book
                    {
                        Autor = "Роберт Мартин",
                        Title = "Чистый код",
                        Category = "прогираммирование"
                    });
                bookDbContext.SaveChanges();
            }
        }
    }
}
