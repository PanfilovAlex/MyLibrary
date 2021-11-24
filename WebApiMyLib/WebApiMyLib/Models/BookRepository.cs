using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using WebApiMyLib.Models.Repository;
using WebApiMyLib.Models.IRepository;
using WebApiMyLib.Controllers;

namespace WebApiMyLib.Models
{
    public class BookRepository : IBookRepository
    {
        private BookDbContext bookContext;

        public BookRepository(BookDbContext context)
        {
            bookContext = context;
        }

        public IEnumerable<Book> GetBooks => bookContext.Books;
        public IEnumerable<Book> Books (PageParameters pageParameters)
        {
           var books = bookContext.Books
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
            return books
                .OrderBy(a => a.Id)
                .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                .Take(pageParameters.PageSize)
                .ToList();
        }
        public Book AddBook(Book book)
        {
            var autors = bookContext.Autors.Where(a => book.Autors.Select(bId => bId.Id).Contains(a.Id)).ToList();
            var categoies = bookContext.Categories.Where(c => book.Categories.Select(cId => cId.Id).Contains(c.Id)).ToList();
            var newBook = new Book()
            {
                Title = book.Title,
                Autors = autors,
                Categories = categoies
            };
            bookContext.Books.Add(newBook);
            bookContext.SaveChanges();
            return newBook;
        }

        public void DeleteBook(int id)
        {
            var deletedBook = bookContext.Books.Find(id);
            bookContext.Books.Find(id).IsDeleted = true;
            bookContext.SaveChanges();
        }

        public Book Find(int id)
        {
            var foundBook = bookContext.Books.Where(b => !b.IsDeleted).FirstOrDefault(b => b.Id == id);
            return foundBook;
        }

        public Book UpdateBook(Book book)
        {
            var updatedBook = bookContext.Books.FirstOrDefault(b => b.Id == book.Id);
            var autors = bookContext.Autors.Where(a => book.Autors.Select(bId => bId.Id).Contains(a.Id)).ToList();
            var categoies = bookContext.Categories.Where(c => book.Categories.Select(cId => cId.Id).Contains(c.Id)).ToList();
            if (updatedBook != null)
            {
                updatedBook.Title = book.Title;
                updatedBook.Autors = autors;
                updatedBook.Categories = categoies;
                updatedBook.IsDeleted = book.IsDeleted;
            }
            bookContext.SaveChanges();
            return updatedBook;
        }
    }
}
