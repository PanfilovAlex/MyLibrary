using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using WebApiMyLib.Models.Repository;
using WebApiMyLib.Models.IRepository;

namespace WebApiMyLib.Models
{
    public class BookRepository : IBookRepository
    {
        private BookDbContext bookContext;

        public BookRepository(BookDbContext context)
        {
            bookContext = context;
        }

        public IEnumerable<Book> Books => bookContext.Books
            .Where(book => !book.IsDeleted)
            .Include(c => c.Categories)
            .Include(a => a.Autors)
            .Select(b => new Book
            {
                Id = b.Id,
                Title = b.Title,
                IsDeleted = b.IsDeleted,
                Categories = b.Categories.Select(c => new Category
                {
                    Id = c.Id,
                    Name = c.Name,
                }).ToList(),
                Autors = b.Autors.Select(a => new Autor
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    LastName = a.LastName
                }).ToList()
            });


        public Book AddBook(Book book)
        {
            var autors = new List<Autor>();
            var categoies = new List<Category>();
            var newBook = new Book();
            var currentBookAutors = book.Autors.ToList();
            var currentBookCategory = book.Categories.ToList();
            for (int i = 0; i < currentBookAutors.Count; i++)
            {
                autors.Add(bookContext.Autors
                    .FirstOrDefault(a =>
                    a.Id == currentBookAutors[i].Id));
            }
            for (int i = 0; i < currentBookCategory.Count; i++)
            {
                categoies.Add(bookContext.Categories
                    .FirstOrDefault(c =>
                    c.Id == currentBookCategory[i].Id));
            }
            newBook.Title = book.Title;
            newBook.Autors = autors;
            newBook.Categories = categoies;

            bookContext.Books.Add(newBook);
            bookContext.SaveChanges();
            return newBook;
        }

        public void DeleteBook(int id)
        {
            var deletedBook = bookContext.Books.Find(id);
            if (deletedBook is null)
            {
                throw new Exception("Unable to find the book!");
            }
            bookContext.Books.Find(id).IsDeleted = true;
            bookContext.SaveChanges();
        }

        public Book Find(int id)
        {
            var foundBook = bookContext.Find<Book>(id);
            if (foundBook.IsDeleted)
            {
                throw new Exception("Book was not found");
            }
            return foundBook;
        }


        public Book UpdateBook(Book book)
        {
            var updatedBook = bookContext.Books.FirstOrDefault();
            if (updatedBook != null)
            {
                updatedBook.Title = book.Title;

            }
            bookContext.SaveChanges();
            return updatedBook;
        }


    }
}
