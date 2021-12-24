using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApiMyLib.Models;
using Microsoft.EntityFrameworkCore;
using WebApiMyLib.Data.Models;
using WebApiMyLib.Data.Repositories;

namespace WebApiMyLib.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private IBookRepository _bookRepository;
        private IAuthorRepository _autorRepository;
        public BookController(IBookRepository bookRepository, IAuthorRepository autorRepository)
        {
            _bookRepository = bookRepository;
            _autorRepository = autorRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<BookDto>> Get([FromQuery] BookPageParameters pageParameters)
        {
            var books = _bookRepository.Books(pageParameters).ToList();
            return books.Select(book => ConvertToBookDto(book)).ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<BookDto> Get(int id)
        {
            var book = _bookRepository.Find(id);
            if (book == null)
                return NotFound();
            var bookDto = ConvertToBookDto(book);
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
                Authors = autorsFromDb
            };
            //требует рефакторнига - выглядит убого
            newBook =_bookRepository.AddBook(newBook);
            return Ok(newBook);
        }

        [HttpPut]
        public ActionResult<Book> Put([FromBody] Book book)
        {
            var authorsFromBook = CheckOrCreateAutor(book);
            var newBook = new Book
            {
                Id = book.Id,
                Title = book.Title,
                Authors = authorsFromBook,
                Categories = book.Categories
            };
            var updatedBook = _bookRepository.UpdateBook(newBook);
            if (updatedBook == null)
            {
                return BadRequest();
            }
            return Ok(updatedBook);
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

        private List<Author> CheckOrCreateAutor(Book book)
        {
            var autorsFromBook = book.Authors;
            var existingAuthorIds = new List<Author>();
            var checkedAutor = _autorRepository.GetAuthors
                   .Where((a) => autorsFromBook.Select((afb) => afb.LastName).Contains(a.LastName)
                   && autorsFromBook.Select((afb) => afb.FirstName).Contains(a.FirstName))
                   .ToList();
            existingAuthorIds.AddRange(checkedAutor);
            foreach (var autor in autorsFromBook)
            {
                if(!checkedAutor.Any(a => a.LastName == autor.LastName && a.FirstName == autor.FirstName ))
                {
                    existingAuthorIds.Add(_autorRepository.Add(autor));
                }
            }
            return existingAuthorIds;
        }

        private BookDto ConvertToBookDto(Book book)
        {
            var bookDto = new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Authors = book.Authors.Select(b => new AuthorDto
                {
                    Id = b.Id,
                    FirstName = b.FirstName,
                    LastName = b.LastName
                }).ToList(),
                Categories = book.Categories.Select(c => new CategoryDto
                {
                    Name = c.Name
                }).ToList()
            };
            return bookDto;
        }
    }
}
