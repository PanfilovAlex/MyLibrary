using System.Collections.Generic;
using System.Linq;
using WebApiMyLib.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;

namespace WebApiMyLib.Data.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private BookDbContext _autorContext;

        public AuthorRepository(BookDbContext context) => _autorContext = context;

        public IEnumerable<Author> GetAuthors => _autorContext.Authors;
        public IEnumerable<Author> Authors(BookPageParameters pageParameters, 
            Expression<Func<Author, bool>> expression)
        {
            var authors = _autorContext.Authors.Where(expression)
                .Include(books => books.Books.Where(book => !book.IsDeleted));

            return PagedList<Author>.ToPagedList(authors, pageParameters.PageNumber, pageParameters.PageSize);
        }
        public Author Add(Author autor)
        {
            var addedAutor = new Author
            {
                FirstName = autor.FirstName,
                LastName = autor.LastName
            };
            _autorContext.Add(addedAutor);
            _autorContext.SaveChanges();
            return addedAutor;
        }

        public void Delete(int id)
        {
            var deletedAutro = _autorContext.Authors.Find(id);
            deletedAutro.IsDeleted = true;
            _autorContext.SaveChanges();
        }

        public Author Find(int id) => _autorContext.Authors.FirstOrDefault(a => a.Id == id);

        public Author Update(Author autor)
        {
            var originalAutor = _autorContext.Authors.Include(b => b.Books).FirstOrDefault(a => a.Id == autor.Id);
            
            if (originalAutor != null)
            {
                originalAutor.FirstName = autor.FirstName;
                originalAutor.LastName = autor.LastName;
                originalAutor.IsDeleted = autor.IsDeleted;
                originalAutor.Books = autor.Books.Select(a => new Book
                {
                    Id = a.Id,
                    Title = a.Title,
                    IsDeleted = a.IsDeleted,
                    Categories = a.Categories,
                    Authors = a.Authors
                }).ToList();
                _autorContext.SaveChanges();
            }
            return originalAutor;
        }
    }
}
