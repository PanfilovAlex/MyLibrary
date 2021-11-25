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
        public IEnumerable<Book> Books (BookPageParameters pageParameters)
        {
           var books = bookContext.Books
            .Where(book => !book.IsDeleted)
            .OrderBy(b => b.Id)
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

            SearchString(ref books, pageParameters.SearchString);
            SortBy(ref books, pageParameters.SortBy);
            return PagedList<Book>.ToPagedList(books, pageParameters.PageNumber, pageParameters.PageSize);
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

        private void SearchString(ref IQueryable<Book> books, string searchString)
        {
            if (!books.Any() || string.IsNullOrWhiteSpace(searchString))
                return;
            books = books.Where(b => b.Title.Contains(searchString)
            || b.Autors.Select(a => a.LastName).Contains(searchString)
            || b.Autors.Select(a => a.FirstName).Contains(searchString));
        }

        private void SortBy(ref IQueryable<Book> books, string sortBy)
        {
            if (!books.Any() || string.IsNullOrWhiteSpace(sortBy))
                return;
            switch (sortBy)
            {
                case "asc":
                    books = books.OrderBy(b => b.Title);
                    break;
                case "desc":
                    books = books.OrderByDescending(b => b.Title);
                    break;
                default:
                    books = books.OrderBy(b => b.Id);
                    break;
            }
        }
    }
}
