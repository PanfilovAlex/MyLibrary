using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApiMyLib.Data.Repositories;
using WebApiMyLib.Data.Models;

namespace WebApiMyLib.Controllers
{
    [Route("api/authors")]
    public class AuthorController : ControllerBase
    {
        private IAutorRepository _autorRepository;

        public AuthorController(IAutorRepository repository) => _autorRepository = repository;

        [HttpGet]
        public ActionResult<IEnumerable<Author>> Get([FromQuery] BookPageParameters pageParameters)
           => _autorRepository.Autors(pageParameters).ToList();

        [HttpGet("{id}")]
        public ActionResult<Author> Get(int id) =>
            (_autorRepository.Find(id) == null) ? NotFound() : Ok(_autorRepository.Find(id));

        [HttpPost]
        public ActionResult<Author> Post(Author autor)
        {
            var updatedAutor = _autorRepository.Add(autor);
            if (updatedAutor == null)
            {
                return BadRequest();
            }
            return Ok($"{updatedAutor} was added");
        }

        
        public IActionResult Delete(int id)
        {
            var exsistingAutor = _autorRepository.GetAutors.FirstOrDefault(a => a.Id == id);
            if (exsistingAutor == null)
            {
                return BadRequest();
            }
            _autorRepository.Delete(id);
            return Ok();
        }
    }
}
