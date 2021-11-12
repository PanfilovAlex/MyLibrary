using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace WebApiMyLib.Models
{
    public class BookRepository : IBookRepository
    {
        private BookDbContext bookDbContext;

        public BookRepository(BookDbContext context)
        {
            bookDbContext = context;
        }

        public IEnumerable<Book> Books => bookDbContext.Books
            .Where(book => !book.IsDeleted)
            .Include(c => c.Categories)
            .ToList();
        

        public Book AddBook(Book book)
        {
            var newBook = new Book
            {
               
            };
            bookDbContext.Add(newBook);
            bookDbContext.SaveChanges();
            return newBook;
        }

        public void DeleteBook(int id)
        {
            var deletedBook = bookDbContext.Books.Find(id);
            if (deletedBook is null)
            {
                throw new Exception("Unable to find the book!");
            }
            bookDbContext.Books.Find(id).IsDeleted = true;
            bookDbContext.SaveChanges();
        }

        public Book Find(int id)
        {
            var foundBook = bookDbContext.Find<Book>(id);
            if (foundBook.IsDeleted)
            {
                throw new Exception("Book was not found");
            }
            return foundBook;
        }


        public Book UpdateBook(Book book)
        {
            var updatedBook = bookDbContext.Books.FirstOrDefault();
            if (updatedBook != null)
            {
                updatedBook.Title = book.Title;
               
            }
            bookDbContext.SaveChanges();
            return updatedBook;
        }


    }
}
