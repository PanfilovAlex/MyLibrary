using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiMyLib.Models;
using Microsoft.AspNetCore.JsonPatch;
using WebApiMyLib.Models.IRepository;

namespace WebApiMyLib.Controllers
{
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private IBookRepository _bookRepository;
        private IAutorRepository _autorRepository;
        public BookController(IBookRepository bookRepository, IAutorRepository autorRepository)
        {
            _bookRepository = bookRepository;
            _autorRepository = autorRepository;
        }

        [HttpGet]
        public IEnumerable<Book> Get() => _bookRepository.Books;

        [HttpGet("{id}")]
        public Book Get(int id) => _bookRepository.Find(id);


        [HttpPost]
        public Book Post([FromBody] Book book)
        {
            var findAutor = book.Autors.ToArray();
            var newBook = new Book()
            {
                Title = book.Title,
                IsDeleted = book.IsDeleted,
                Categories = book.Categories,
            };

            for (int i = 0; i < findAutor.Length; i++)
            {
                var result = CheckAutor(findAutor[i]);
                newBook.Autors.Add(new Autor
                {
                    Id = _autorRepository.Find(result).Id,
                    FirstName = _autorRepository.Find(result).FirstName,
                    LastName = _autorRepository.Find(result).LastName,
                });
            }
            _bookRepository.AddBook(newBook);

            return newBook;
        }

        [HttpPut]
        public Book Put([FromBody] Book book) => _bookRepository.UpdateBook(book);

        [HttpPatch("{id}")]
        public StatusCodeResult Patch(int id, [FromBody] JsonPatchDocument<Book> patch)
        {
            Book book = Get(id);
            if (book != null)
            {
                patch.ApplyTo(book);
                return Ok();
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        public void Delete(int id) => _bookRepository.DeleteBook(id);

        private int CheckAutor(Autor autor)
        {
            var checkedAutor = _autorRepository.Autors
                .FirstOrDefault(a => autor.FirstName.Equals(a.FirstName) && autor.LastName.Equals(a.LastName));
            if (checkedAutor == null)
            {
                _autorRepository.Add(autor);
                checkedAutor = _autorRepository.Autors
                .FirstOrDefault(a => autor.FirstName.Equals(a.FirstName) && autor.LastName.Equals(a.LastName));
            }

            return checkedAutor.Id;
        }

        private int CheckAutor(Book book)
        {
            var checkedAutor = _autorRepository.Autors
                .FirstOrDefault(a =>
                a.LastName.Equals(book.Autors.FirstOrDefault().LastName)
                && a.FirstName.Equals(book.Autors.FirstOrDefault().FirstName));

            if (checkedAutor == null)
            {
                _autorRepository.Add(checkedAutor);
            }

            return checkedAutor.Id;

        }

    }
}
