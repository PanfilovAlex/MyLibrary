using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApiMyLib.Data.Repositories;
using WebApiMyLib.Data.Models;
using WebApiMyLib.Models;

namespace WebApiMyLib.Controllers
{
    [Route("api/authors")]
    public class AuthorController : ControllerBase
    {
        private IAuthorRepository _authorRepository;

        public AuthorController(IAuthorRepository repository) => _authorRepository = repository;

        [HttpGet]
        public ActionResult<IEnumerable<AuthorDto>> Get([FromQuery] BookPageParameters pageParameters)
        {
            var authors = _authorRepository.Authors(pageParameters);
            return authors.Select(autor => ConvertToAuthorDto(autor)).ToList();       
        }

        [HttpGet("{id}")]
        public ActionResult<AuthorDto> Get(int id)
        {
            var autor = _authorRepository.Find(id);
            if(autor == null)
            {
                return BadRequest();
            }
            return ConvertToAuthorDto(autor);
        }

        [HttpPost]
        public ActionResult<Author> Post([FromBody]Author autor)
        {
            var adeddAuthor = _authorRepository.Add(autor);
            if (adeddAuthor == null)
            {
                return BadRequest();
            }
            return Ok($"{adeddAuthor} was added");
        }

        [HttpPut]
        public ActionResult<Author> Put([FromBody]Author author)
        {
            var updatedAuthor = _authorRepository.Update(author);
            if(updatedAuthor == null)
            {
                return BadRequest();
            }
            return Ok(updatedAuthor);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var exsistingAutor = _authorRepository.GetAuthors.FirstOrDefault(a => a.Id == id);
            if (exsistingAutor == null)
            {
                return BadRequest();
            }
            _authorRepository.Delete(id);
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
