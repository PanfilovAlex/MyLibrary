using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiMyLib.Models;
using WebApiMyLib.Data.Models;
using WebApiMyLib.BLL.Interfaces;

namespace WebApiMyLib.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private IBookService _bookService;
        private IAuthorService _autorService;
        public BookController(IBookService bookService, IAuthorService autorService)
        {
            _bookService = bookService;
            _autorService = autorService;
        }

        [HttpGet]
        public ActionResult<PagedListDto<BookDto>> Get([FromQuery] BookPageParameters pageParameters)
        {
            var books = _bookService.Books(pageParameters);
                        
            return new PagedListDto<BookDto>(books.Select((b) => ConvertToBookDto(b)).ToList(), books.MetaData.TotalCount);
        }

        [HttpGet("{id}")]
        public ActionResult<BookDto> Get(int id)
        {
            var book = _bookService.Find(id);
            if (book == null)
            { 
                return NotFound();
            }
            var bookDto = ConvertToBookDto(book);

            return Ok(bookDto);
        }

        [Authorize(Roles ="admin")]
        [HttpPost]
        public ActionResult<Book> Post([FromBody] Book book)
        {
            var autorsFromDb = CheckOrCreateAutor(book);
            var newBook = new Book()
            {
                Title = book.Title,
                IsDeleted = book.IsDeleted,
                Categories = book.Categories,
                Authors = autorsFromDb
            };
            //требует рефакторнига - выглядит убого
            newBook = _bookService.Add(newBook);
            
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
            var updatedBook = _bookService.Update(newBook);
            
            if (updatedBook == null)
            {
                return BadRequest();
            }

            return Ok(updatedBook);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var exsitingBook = _bookService.Find(id);
            if (exsitingBook == null)
            {
                return NotFound();
            }
            _bookService.Delete(id);
            return Ok("Book was deleted");
        }

        private List<Author> CheckOrCreateAutor(Book book)
        {
            var autorsFromBook = book.Authors;
            var existingAuthorIds = new List<Author>();
            var checkedAutor = _autorService.GetAuthors
                   .Where((a) => autorsFromBook.Select((afb) => afb.LastName).Contains(a.LastName)
                   && autorsFromBook.Select((afb) => afb.FirstName).Contains(a.FirstName))
                   .ToList();
            existingAuthorIds.AddRange(checkedAutor);
            foreach (var autor in autorsFromBook)
            {
                if (!checkedAutor.Any(a => a.LastName == autor.LastName && a.FirstName == autor.FirstName))
                {
                    existingAuthorIds.Add(_autorService.Add(autor));
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
