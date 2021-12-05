using System.Collections.Generic;
using System.Linq;
using WebApiMyLib.Data.Models;

namespace WebApiMyLib.Data.Repositories
{
    public class AutorRepository : IAutorRepository
    {
        private BookDbContext _autorContext;

        public AutorRepository(BookDbContext context) => _autorContext = context;

        public IEnumerable<Author> GetAutors => _autorContext.Authors;
        public IEnumerable<Author> Autors(PageParameters pageParameters)
        {
            return _autorContext.Authors.OrderBy(a => a.Id)
                 .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                 .Take(pageParameters.PageSize);
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
            var updatedAutor = _autorContext.Authors.FirstOrDefault(a => a.Id == autor.Id);
            if (updatedAutor != null)
            {
                updatedAutor.FirstName = autor.FirstName;
                updatedAutor.LastName = autor.LastName;
                updatedAutor.IsDeleted = autor.IsDeleted;
                updatedAutor.Books = autor.Books.Select(a => new Book
                {
                    Title = a.Title
                }).ToList();
                _autorContext.SaveChanges();
            }
            return updatedAutor;
        }
    }
}
