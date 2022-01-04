using System.Collections.Generic;
using WebApiMyLib.Data.Models;

namespace WebApiMyLib.Data.Repositories
{
    public interface IBookRepository
    {
        IEnumerable<Book> GetBooks { get; }
        PagedList<Book> Books(BookPageParameters pageParameters);
        Book Find(int id);
        Book AddBook(Book book);
        Book UpdateBook(Book book);
        void DeleteBook(int id);
    }
}
