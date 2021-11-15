using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiMyLib.Models.IRepository;

namespace WebApiMyLib.Models.Repository
{
    public class AutorRepository : IAutorRepository
    {
        private BookDbContext _autorContext;

        public AutorRepository(BookDbContext context) => _autorContext = context;
        public IEnumerable<Autor> Autors => _autorContext.Autors;
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

        public Autor Find(int id)
        {
            return _autorContext.Autors.FirstOrDefault(a => a.Id == id);
        }

        public Autor Update(Autor autor)
        {
            var updatedAutor = _autorContext.Autors.Find(autor.Id);
            updatedAutor.FirstName = autor.FirstName;
            updatedAutor.LastName = autor.LastName;
            updatedAutor.IsDeleted = autor.IsDeleted;
            updatedAutor.Books = autor.Books?.Select(a => new Book
            {
                Title = a.Title
            }).ToList();
            _autorContext.SaveChanges();
            return updatedAutor;
        }
    }
}
