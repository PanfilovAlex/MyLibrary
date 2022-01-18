using System.Collections.Generic;
using WebApiMyLib.Data.Models;
using System;
using System.Linq.Expressions;

namespace WebApiMyLib.Data.Repositories
{
    public interface IBookRepository
    {
        IEnumerable<Book> GetBooks { get; }
        PagedList<Book> Books(BookPageParameters pageParameters, Expression<Func<Book, bool>> expression);
        Book Find(int id);
        Book AddBook(Book book);
        Book UpdateBook(Book book);
        void DeleteBook(int id);
    }
}
