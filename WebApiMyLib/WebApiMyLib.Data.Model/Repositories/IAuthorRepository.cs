using System.Collections.Generic;
using WebApiMyLib.Data.Models;

namespace WebApiMyLib.Data.Repositories
{
    public interface IAuthorRepository
    {
        IEnumerable<Author> GetAuthors { get; }
        IEnumerable<Author> Authors(BookPageParameters pageParameters);
        public Author Find(int id);
        public Author Add(Author autor);
        public Author Update(Author autor);
        public void Delete(int id);
    }
}
