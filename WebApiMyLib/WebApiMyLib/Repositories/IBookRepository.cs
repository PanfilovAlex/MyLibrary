using System.Collections.Generic;
using WebApiMyLib.Controllers;
using WebApiMyLib.Models;

namespace WebApiMyLib.Repositories
{
    public interface IBookRepository
    {
        
        IEnumerable<Book> GetBooks { get; }
        IEnumerable<Book> Books(BookPageParameters pageParameters);
        Book Find(int id);
        Book AddBook(Book book);
        Book UpdateBook(Book book);
        void DeleteBook(int id);
    }
}
