
using System.Collections.Generic;

namespace WebApiMyLib.Models
{
    public class MemoryRepository:IBookRepository
    {
        private Dictionary<int, Book> books;

        public MemoryRepository()
        {
            books = new Dictionary<int, Book>();
            new List<Book>
            {
                new Book {Title = "ASP.NET MVC 2 для профессионалов", Autor = "Адам Фримен", Category = "C#"},
                new Book {Title = "C# 7 и Asp.Net", Autor = "Троелсон", Category ="C#"},
                new Book {Title = "C++  самоучитель", Category = "C++", Autor = "Бьерн Страуструп"}
            }.ForEach(r => AddBook(r));
        }

        public Book this[int id] => books.ContainsKey(id)?books[id] : null;
        public IEnumerable<Book> Books => books.Values;
        public Book AddBook(Book book)
        {
            if(book.BookId == 0)
            {
                int key = books.Count;
                while (books.ContainsKey(key)) { key++; };
                book.BookId = key;
            }
            books[book.BookId] = book;
            return book;
        }
        public void DeleteBook(int id) => books.Remove(id);
        public Book UpdateBook(Book book) => AddBook(book);



    }
}
