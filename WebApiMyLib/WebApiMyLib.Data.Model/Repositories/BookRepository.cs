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
             .Include(a => a.Authors);

            var searchedBooks = ApplySearchString(books, pageParameters.SearchString);
            var sortedBooks = SortBy(searchedBooks, pageParameters.SortBy);

            return PagedList<Book>.ToPagedList(sortedBooks, pageParameters.PageNumber, pageParameters.PageSize);
        }
        public Book AddBook(Book book)
        {
            var autors = bookContext.Authors.Where(a => book.Authors.Select(bId => bId.Id).Contains(a.Id)).ToList();
            var categoies = bookContext.Categories.Where(c => book.Categories.Select(cId => cId.Id).Contains(c.Id)).ToList();
            var newBook = new Book()
            {
                Title = book.Title,
                Authors = autors,
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
                .Include(a => a.Authors)
                .Include(c => c.Categories)
                .FirstOrDefault(b => b.Id == id);
            return foundBook;
        }

        public Book UpdateBook(Book book)
        {
            var updatedBook = bookContext.Books.Include(a => a.Authors).Include(c => c.Categories).FirstOrDefault(i => i.Id == book.Id);
            
            var authors = bookContext.Authors.Where(a => book.Authors.Select(bId => bId.Id).Contains(a.Id)).ToList();
            var categoies = bookContext.Categories.Where(c => book.Categories.Select(cId => cId.Id).Contains(c.Id)).ToList();
            
            if (updatedBook != null)
            {
                updatedBook.Title = book.Title;
                updatedBook.Authors = authors;
                updatedBook.Categories = categoies;
                updatedBook.IsDeleted = book.IsDeleted;
            }
            bookContext.SaveChanges();
            return updatedBook;
        }

        private IQueryable<Book> ApplySearchString(IQueryable<Book> books, string searchString)
        {
            if (!books.Any() || string.IsNullOrWhiteSpace(searchString))
            {
                return books;
            }

            return books.Where(b => b.Title.Contains(searchString)
            || b.Authors.Select(a => a.LastName).Contains(searchString)
            || b.Authors.Select(a => a.FirstName).Contains(searchString));
        }

        private IQueryable<Book> SortBy(IQueryable<Book> books, string sortBy)
        {
            if (!books.Any() || string.IsNullOrWhiteSpace(sortBy))
            { 
                return books; 
            }

            switch (sortBy)
            {
                case "asc":
                    books = books.OrderBy(b => b.Title);
                    break;
                case "desc":
                    books = books.OrderByDescending(b => b.Title);
                    break;
                default:
                    break;
            }
            return books;
        }
        private List<Author> CheckOrCreateAutor(Book book)
        {
            var autorsFromBook = book.Authors;
            var existingAuthorIds = new List<Author>();
            var checkedAutor = bookContext.Authors
                   .Where((a) => autorsFromBook.Select((afb) => afb.LastName).Contains(a.LastName)
                   && autorsFromBook.Select((afb) => afb.FirstName).Contains(a.FirstName)).ToList();
            if(checkedAutor != null)
            {
                existingAuthorIds.AddRange(checkedAutor);
            }
            foreach (var autor in autorsFromBook)
            {
                if(autor.FirstName!.Equals(checkedAutor.FirstOrDefault().FirstName) 
                    && autor.LastName!.Equals(checkedAutor.FirstOrDefault().LastName))
                {
                    bookContext.Authors.Add(new Author
                    {
                        FirstName = autor.FirstName,
                        LastName = autor.LastName
                    });
                    bookContext.SaveChanges();
                    existingAuthorIds.Add(bookContext.Authors.FirstOrDefault(a => a.LastName.Equals(autor.LastName)
                    && a.FirstName.Equals(autor.FirstName)));
                }    
            }
            return existingAuthorIds;
        }
    }
}
