using System.Collections.Generic;
using WebApiMyLib.Data.Models;

namespace WebApiMyLib.BLL.Interfaces
{
    public interface IBookService
    {
        IEnumerable<Book> GetBooks { get; }
        IEnumerable<Book> Books(BookPageParameters pageParameters);
        Book Find(int id);
        Book Add(Book book);
        Book Update(Book book);
        void Delete(int id);
    }
}
