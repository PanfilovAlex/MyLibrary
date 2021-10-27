using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiMyLib.Models
{
    interface IBookRepository
    {
        IEnumerable<Book> Books { get; }
        Book this[int id] { get; }
        Book AddBook(Book book);
        Book UpdateBook(Book book);
        void DeleteBook(int id);
    }
}
