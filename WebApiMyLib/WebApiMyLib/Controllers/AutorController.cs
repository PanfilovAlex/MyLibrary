using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiMyLib.Models;
using WebApiMyLib.Models.IRepository;

namespace WebApiMyLib.Controllers
{
    [Route("api/autors")]
    public class AutorController:ControllerBase
    {
        private IAutorRepository _autorRepository;

        public AutorController(IAutorRepository repository) => _autorRepository = repository;

        [HttpGet]
        public IEnumerable<Autor> Get() => _autorRepository.Autors;

        [HttpGet("{id}")]
        public Autor Get(int id) => _autorRepository.Find(id);

        //[HttpPost]
        //public Autor Post(Autor autor)
        //{

        //}
        
    }
}
