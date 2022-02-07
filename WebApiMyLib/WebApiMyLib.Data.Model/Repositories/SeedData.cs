using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using WebApiMyLib.Data.Models;

namespace WebApiMyLib.Data.Repositories
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var bookDbContext = new BookDbContext(
               serviceProvider.GetRequiredService<
                   DbContextOptions<BookDbContext>>()))
            {
                if(!bookDbContext.Users.Any())
                {
                    User admin = new User
                    {
                        UserName = "admin",
                        Password = "1234",
                        Role = "admin"
                    };
                    bookDbContext.Users.Add(admin);
                    bookDbContext.SaveChanges();
                }
                // Look for any movies.
                if (bookDbContext.Books.Any())
                {
                    return;   // DB has been seeded
                }
                
                Author autor1 = new Author { FirstName = "Адам", LastName = "Фримен" };
                Author autor2 = new Author { FirstName = "Джон Поль", LastName = "Мюллер" };
                Author autor3 = new Author { FirstName = "Лука", LastName = "Массарон" };
                Author autor4 = new Author { FirstName = "Эндрю", LastName = "Троелсен" };
                Author autor5 = new Author { FirstName = "Филипп", LastName = "Джепикс" };
                Author autor6 = new Author { FirstName = "Роберт", LastName = "Мартин" };

                bookDbContext.Authors.AddRange(autor1, autor2, autor3, autor4, autor5, autor6);

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
                    Authors = new Author[]
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
                    Authors = new Author[]
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
                    Authors = new Author[]
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
                    Authors = new Author[]
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
