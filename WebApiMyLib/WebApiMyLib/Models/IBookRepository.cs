using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiMyLib.Controllers;

namespace WebApiMyLib.Models
{
    public interface IBookRepository
    {
        IEnumerable<Book> GetBooks { get; }
        IEnumerable<Book> Books(PageParameters pageParameters);
        Book Find(int id);
        Book AddBook(Book book);
        Book UpdateBook(Book book);
        void DeleteBook(int id);
    }
}
