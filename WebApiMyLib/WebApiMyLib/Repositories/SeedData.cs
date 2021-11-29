using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using WebApiMyLib.Models;

namespace WebApiMyLib.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {

            using (var bookDbContext = new BookDbContext(
               serviceProvider.GetRequiredService<
                   DbContextOptions<BookDbContext>>()))
            {
                // Look for any movies.
                if (bookDbContext.Books.Any())
                {
                    return;   // DB has been seeded
                }
                Autor autor1 = new Autor { FirstName = "Адам", LastName = "Фримен" };
                Autor autor2 = new Autor { FirstName = "Джон Поль", LastName = "Мюллер" };
                Autor autor3 = new Autor { FirstName = "Лука", LastName = "Массарон" };
                Autor autor4 = new Autor { FirstName = "Эндрю", LastName = "Троелсен" };
                Autor autor5 = new Autor { FirstName = "Филипп", LastName = "Джепикс" };
                Autor autor6 = new Autor { FirstName = "Роберт", LastName = "Мартин" };

                bookDbContext.Autors.AddRange(autor1, autor2, autor3, autor4, autor5, autor6);

                Category category1 = new Category { Name = "C#" };
                Category category2 = new Category { Name = "Программирование" };
                Category category3 = new Category { Name = "ASP.NET" };
                Category category4 = new Category { Name = "Информационные технологии" };

                bookDbContext.Categories.AddRange(category1, category2, category3, category4);

                Book book1 = new Book
                {
                    Title = "C# для чайников",
                    Categories = new Category[]
                    {
                        category1,
                        category2,
                        category4
                    }.ToList(),
                    Autors = new Autor[]
                    {
                        autor2
                    }.ToList()
                };
                Book book2 = new Book
                {
                    Title = "Язык программирования C#7 и платформы .NET и NET.Core",
                    Categories = new Category[]
                    {
                        category1,
                        category2,
                        category4,
                        category3
                    }.ToList(),
                    Autors = new Autor[]
                    {
                        autor4,
                        autor5
                    }.ToList()
                };
                Book book3 = new Book
                {
                    Title = "Чистый код",
                    Categories = new Category[]
                    {
                        category2,
                        category4
                    }.ToList(),
                    Autors = new Autor[]
                    {
                        autor6
                    }.ToList()
                };
                Book book4 = new Book
                {
                    Title = "ASP.NET Core MVC 2 с примерами на C# для профессионалов",
                    Categories = new Category[]
                    {
                        category1,
                        category2,
                        category3,
                        category4
                    }.ToList(),
                    Autors = new Autor[]
                    {
                        autor1
                    }.ToList()
                };

                bookDbContext.Books.Add(book1);
                bookDbContext.Books.Add(book2);
                bookDbContext.Books.Add(book3);
                bookDbContext.Books.Add(book4);

                bookDbContext.SaveChanges();


            }
        }
    }
}
