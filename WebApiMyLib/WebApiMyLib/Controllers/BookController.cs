using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiMyLib.Models;
using Microsoft.AspNetCore.JsonPatch;
using WebApiMyLib.Models.IRepository;
using Microsoft.EntityFrameworkCore;

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
            



            var newBook = new Book()
            {
                Title = book.Title,
                IsDeleted = book.IsDeleted,
                Categories = book.Categories,
                
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


        private IEnumerable<Autor> CheckOrCreateAutor(Book book)
        {
            
            var autorsFromBook = book.Autors;
            var id = new List<Autor>();
            var checkedAutor = _autorRepository.Autors
                   .Where((a) => autorsFromBook.Select((afb) => afb.LastName).Contains(a.LastName)
                   && autorsFromBook.Select((afb) => afb.FirstName).Contains(a.FirstName)).ToList();

            foreach (var autor in autorsFromBook)
            {
                if (checkedAutor.Select(a => a.LastName).Contains(autor.LastName) 
                    && checkedAutor.Select(a => a.FirstName).Contains(autor.FirstName))
                {
                    id.Add(checkedAutor.FirstOrDefault(a => a.LastName.Equals(autor.LastName)));
                }
                else
                {
                    id.Add(_autorRepository.Add(autor));
                }
            }

            return id;
        }

    }
}
