using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiMyLib.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using WebApiMyLib.Repositories;



namespace WebApiMyLib.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public ActionResult<IEnumerable<BookDto>> Get([FromQuery] BookPageParameters pageParameters)
        {
            var books = _bookRepository.Books(pageParameters).ToList();
            return books.Select(book => new BookDto
            {
                Title = book.Title,
                Autors = book.Autors.Select(autor => new AutorDto
                {
                    FirstName = autor.FirstName,
                    LastName = autor.LastName
                }).ToList(),
                Categories = book.Categories.Select(category => new CategoryDto
                {
                    Name = category.Name
                }).ToList()
            }).ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<BookDto> Get(int id)
        {
            var book = _bookRepository.Find(id);
            if (book == null)
                return NotFound();
            BookDto bookDto = new BookDto
            {
                Title = book.Title,
                Autors = book.Autors.Select(b => new AutorDto
                {
                    FirstName = b.FirstName,
                    LastName = b.LastName
                }).ToList(),
                Categories = book.Categories.Select(c => new CategoryDto
                {
                    Name = c.Name
                }).ToList()
            };
            return Ok(bookDto);
        }

        [HttpPost]
        public ActionResult<Book> Post([FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var autorsFromDb = CheckOrCreateAutor(book);
            var newBook = new Book()
            {
                Title = book.Title,
                IsDeleted = book.IsDeleted,
                Categories = book.Categories,
                Autors = autorsFromDb
            };
            //требует рефакторнига - выглядит убого
            _bookRepository.AddBook(newBook);
            return Ok("Book was added");
        }

        [HttpPut]
        public ActionResult<Book> Put([FromBody] Book book)
        {
            var updatedBook = _bookRepository.UpdateBook(book);
            if (updatedBook == null)
            {
                return BadRequest();
            }
            return Ok("Book was updated");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var exsitingBook = _bookRepository.Find(id);
            if (exsitingBook == null)
            {
                return NotFound();
            }
            _bookRepository.DeleteBook(id);
            return Ok("Book was deleted");
        }

        private List<Autor> CheckOrCreateAutor(Book book)
        {
            var autorsFromBook = book.Autors;
            var existingAuthorIds = new List<Autor>();
            var checkedAutor = _autorRepository.GetAutors
                   .Where((a) => autorsFromBook.Select((afb) => afb.LastName).Contains(a.LastName)
                   && autorsFromBook.Select((afb) => afb.FirstName).Contains(a.FirstName)).ToList();
            foreach (var autor in autorsFromBook)
            {
                if (checkedAutor.Select(a => a.LastName).Contains(autor.LastName)
                    && checkedAutor.Select(a => a.FirstName).Contains(autor.FirstName))
                {
                    existingAuthorIds.Add(checkedAutor.FirstOrDefault(a => a.LastName.Equals(autor.LastName)));
                }
                else
                {
                    existingAuthorIds.Add(_autorRepository.Add(autor));
                }
            }
            return existingAuthorIds;
        }
    }
}
