using System;
using System.Collections.Generic;
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

        public PagedList<Book> Books(BookPageParameters pageParameters)
        => _bookRepository.Books(pageParameters, book => book.IsDeleted == false);


        public Book Add(Book book)
        {
            var validationResult = _bookValidationService.Validate(book);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }
            try
            {
                return _bookRepository.AddBook(book);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

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
                throw new Exception("Book was not found!");
            }
            return bookToFind;
        }

        public Book Update(Book book)
        {
            var validationResult = _bookValidationService.Validate(book);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }
            try
            {
                return _bookRepository.UpdateBook(book);
            }
            catch(Exception e)
            {
                throw;
            }
        }
    }
}
