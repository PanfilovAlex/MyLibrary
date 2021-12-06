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
        private IAutorRepository _autorRepository;
        public BookController(IBookRepository bookRepository, IAutorRepository autorRepository)
        {
            _bookRepository = bookRepository;
            _autorRepository = autorRepository;
        }

        [HttpGet]
        public ActionResult<PagedListDto<BookDto>> Get([FromQuery] BookPageParameters pageParameters)
        {
            var books = _bookRepository.Books(pageParameters);
            return new PagedListDto<BookDto>(books.Select((b) => ConvertToBookDto(b)).ToList(), books.TotalCount);
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
            var createdBook = _bookRepository.AddBook(newBook);
            return Ok(createdBook);
        }

        [HttpPut]
        public ActionResult<Book> Put([FromBody] Book book)
        {
            var updatedBook = _bookRepository.UpdateBook(book);
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

        private BookDto ConvertToBookDto(Book book)
        {
            var bookDto = new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Authors = book.Authors.Select(b => new AuthorDto
                {
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
