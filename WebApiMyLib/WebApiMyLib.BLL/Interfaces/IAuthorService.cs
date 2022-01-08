using System.Collections.Generic;
using WebApiMyLib.Data.Models;

namespace WebApiMyLib.BLL.Interfaces
{
    public interface IAuthorService
    {
        IEnumerable<Author> GetAuthors { get;  }
        IEnumerable<Author> Authors(BookPageParameters pageParameters);
        Author Add(Author author);
        Author Update(Author author);
        Author Find(int id);
        void Delete(int id);
    }
}
