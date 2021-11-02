using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiMyLib.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace WebApiMyLib.Controllers
{
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private IBookRepository _repository;
        public BookController(IBookRepository bookRepository)
        {
            _repository = bookRepository;
        }

        [HttpGet]
        public IEnumerable<Book> Get() => _repository.Books;

        [HttpGet("{id}")]
        public Book Get(int id) => _repository.Find(id);


        [HttpPost]
        public Book Post([FromBody] Book book) => _repository.AddBook(book);

        [HttpPut]
        public Book Put([FromBody] Book book) => _repository.UpdateBook(book);

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
        public void Delete(int id) => _repository.DeleteBook(id); 
    }
}
