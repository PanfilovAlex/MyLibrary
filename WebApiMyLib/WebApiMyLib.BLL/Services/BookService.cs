using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiMyLib.BLL.Interfaces;
using WebApiMyLib.Data.Models;
using WebApiMyLib.Data.Repositories;

namespace WebApiMyLib.BLL.Services
{
    public class BookService : IBookService
    {
        private IBookRepository _bookRepository;
        private IValidationService<Book> _bookValidationService;

        public BookService(IBookRepository bookRepository, IValidationService<Book> validationService)
        {
            _bookRepository = bookRepository;
            _bookValidationService = validationService;
        }
        public IEnumerable<Book> GetBooks => _bookRepository.GetBooks;

        public Book Add(Book book)
        {
            var validationResult = _bookValidationService.Validate(book);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }
            try
            {
                _bookRepository.AddBook(book);
            }
            catch
            {
                return null;
            }

            return _bookRepository.GetBooks.FirstOrDefault(a => a.Title == book.Title);
        }

        public IEnumerable<Book> Books(BookPageParameters pageParameters)
        => _bookRepository.Books(pageParameters);

        public void Delete(int id)
        {
            var bookToDelete = _bookRepository.Find(id);
            if (bookToDelete == null)
            {
                return;
            }
            _bookRepository.DeleteBook(bookToDelete.Id);
        }

        public Book Find(int id)
        {
            var bookToFind = _bookRepository.Find(id);
            if (bookToFind == null)
            {
                return null;
            }
            return bookToFind;
        }

        public Book Update(Book book)
        {
            var bookToUpdate = new Book();
            var validationResult = _bookValidationService.Validate(book);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }
            try
            {
               bookToUpdate = _bookRepository.UpdateBook(book);
            }
            catch
            {
                return null;
            }

            return bookToUpdate;
        }
    }
}
