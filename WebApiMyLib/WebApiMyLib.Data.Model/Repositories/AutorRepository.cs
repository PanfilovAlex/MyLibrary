using System.Collections.Generic;
using System.Linq;
using WebApiMyLib.Data.Models;

namespace WebApiMyLib.Data.Repositories
{
    public class AutorRepository : IAutorRepository
    {
        private BookDbContext _autorContext;

        public AutorRepository(BookDbContext context) => _autorContext = context;

        public IEnumerable<Autor> GetAutors => _autorContext.Autors;
        public IEnumerable<Autor> Autors(PageParameters pageParameters)
        {
            return _autorContext.Autors.OrderBy(a => a.Id)
                 .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                 .Take(pageParameters.PageSize);
        }
        public Autor Add(Autor autor)
        {
            var addedAutor = new Autor
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
            var deletedAutro = _autorContext.Autors.Find(id);
            deletedAutro.IsDeleted = true;
            _autorContext.SaveChanges();
        }

        public Autor Find(int id) => _autorContext.Autors.FirstOrDefault(a => a.Id == id);

        public Autor Update(Autor autor)
        {
            var updatedAutor = _autorContext.Autors.FirstOrDefault(a => a.Id == autor.Id);
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
