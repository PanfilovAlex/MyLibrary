using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiMyLib.BLL.Interfaces;
using WebApiMyLib.Data.Models;

namespace WebApiMyLib.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]

    public class CategoryController : ControllerBase
    {
        private ICategoryService _categoryService;
        public CategoryController(ICategoryService repository)
        {
            _categoryService = repository;
        }

        [HttpGet]
        public IEnumerable<Category> Get() 
        { 
            return _categoryService.Categories;
        }

        [HttpGet("{id}")]
        public Category Get(int id)
        {
           return _categoryService.Find(id);
        } 

        [Authorize(Roles = "admin")]
        [HttpPost]
        public Category Post([FromBody]Category category)
        { 
            return _categoryService.Add(category);
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        public Category Put([FromBody]Category category)
        {
            return _categoryService.Update(category);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _categoryService.Delete(id);
        }
    }
}
