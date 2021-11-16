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

            var autorsIdDb = CheckOrCreateAutor(book);
            var autors = new List<Autor>();

            foreach (var i in autorsIdDb)
            {
                autors.Add(_autorRepository.Autors.FirstOrDefault(a => a.Id == i));
            }


            var newBook = new Book()
            {
                Title = book.Title,
                IsDeleted = book.IsDeleted,
                Categories = book.Categories,
                Autors = autors
            };


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


        private IEnumerable<int> CheckOrCreateAutor(Book book)
        {
            var autorsFromBook = book.Autors;
            var id = new List<int>();
            var checkedAutor = _autorRepository.Autors
                   .Where(a =>
                   a.LastName.Equals(book.Autors.FirstOrDefault().LastName, StringComparison.CurrentCultureIgnoreCase)
                   && a.FirstName.Equals(book.Autors.FirstOrDefault().FirstName, StringComparison.CurrentCultureIgnoreCase))
                   .ToList();

            foreach (var autor in autorsFromBook)
            {
                if (autor.LastName.Equals(checkedAutor.FirstOrDefault().LastName, StringComparison.CurrentCultureIgnoreCase)
                    && autor.FirstName.Equals(checkedAutor.FirstOrDefault().FirstName, StringComparison.CurrentCultureIgnoreCase))
                {
                    id.Add(checkedAutor.FirstOrDefault().Id);
                }
                else
                {
                    id.Add(_autorRepository.Add(autor).Id);
                }
            }

            return id;
        }

    }
}
