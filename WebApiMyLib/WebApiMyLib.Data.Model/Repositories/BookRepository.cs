using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using WebApiMyLib.Data.Models;


namespace WebApiMyLib.Data.Repositories
{
    public class BookRepository : IBookRepository
    {
        private BookDbContext bookContext;

        public BookRepository(BookDbContext context)
        {
            bookContext = context;
        }

        public IEnumerable<Book> GetBooks => bookContext.Books;
        public IEnumerable<Book> Books(BookPageParameters pageParameters)
        {
            var books = bookContext.Books
             .Where(book => !book.IsDeleted)
             .OrderBy(b => b.Id)
             .Include(c => c.Categories)
             .Include(a => a.Autors)
             .ToList();

            var searchedBooks = SearchString(books, pageParameters.SearchString);
            var sortedBooks = SortBy(searchedBooks, pageParameters.SortBy).ToList();
            
            return PagedList<Book>.ToPagedList(sortedBooks, pageParameters.PageNumber, pageParameters.PageSize);
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
            var foundBook = bookContext.Books
                .Where(b => !b.IsDeleted)
                .Include(a => a.Autors)
                .Include(c => c.Categories)
                .FirstOrDefault(b => b.Id == id);
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

        private List<Book> SearchString(List<Book> books, string searchString)
        {
            if (!books.Any() || string.IsNullOrWhiteSpace(searchString))
                return books;
            return books.Where(b => b.Title.Contains(searchString)
            || b.Autors.Select(a => a.LastName).Contains(searchString)
            || b.Autors.Select(a => a.FirstName).Contains(searchString)).ToList();
        }

        private List<Book> SortBy(List<Book> books, string sortBy)
        {
            if (!books.Any() || string.IsNullOrWhiteSpace(sortBy))
                return books;
            switch (sortBy)
            {
                case "asc":
                    books = books.OrderBy(b => b.Title).ToList();
                    break;
                case "desc":
                    books = books.OrderByDescending(b => b.Title).ToList();
                    break;
                default:
                    break;
            }
            return books;
        }
    }
}
