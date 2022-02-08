using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApiMyLib.Data.Repositories;
using WebApiMyLib.Data.Models;
using WebApiMyLib.Models;
using WebApiMyLib.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace WebApiMyLib.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private IAuthorService _authorService;

        public AuthorController(IAuthorService authorService) => _authorService = authorService;

        [HttpGet]
        public ActionResult<IEnumerable<AuthorDto>> Get([FromQuery] BookPageParameters pageParameters)
        {
            var authors = _authorService.Authors(pageParameters);
            return authors.Select(autor => ConvertToAuthorDto(autor)).ToList();       
        }

        [HttpGet("{id}")]
        public ActionResult<AuthorDto> Get(int id)
        {
            var autor = _authorService.Find(id);
            if(autor == null)
            {
                return BadRequest();
            }
            return ConvertToAuthorDto(autor);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult<Author> Post([FromBody]Author author)
        {
            var adeddAuthor = _authorService.Add(author);
            if (adeddAuthor == null)
            {
                return BadRequest();
            }
            
            return Ok(ConvertToAuthorDto(adeddAuthor));
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        public ActionResult<Author> Put([FromBody]Author author)
        {
            var updatedAuthor = _authorService.Update(author);
            if (updatedAuthor == null)
            {
                return BadRequest();
            }

            return Ok(updatedAuthor);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var exsistingAutor = _authorService.Find(id);
            if (exsistingAutor == null)
            {
                return BadRequest();
            }

            _authorService.Delete(id);
            return Ok();
        }

        private AuthorDto ConvertToAuthorDto(Author author)
        {
            var newAuthorDto = new AuthorDto
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                BooksDto = author.Books.Select(books => new BookDto
                {
                    Id = books.Id,
                    Title = books.Title
                }).ToList()
            };
            return newAuthorDto;
        }
    }
}
