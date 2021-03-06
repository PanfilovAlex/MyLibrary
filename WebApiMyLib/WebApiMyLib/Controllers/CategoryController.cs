using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebApiMyLib.BLL.Interfaces;
using WebApiMyLib.Data.Models;

namespace WebApiMyLib.Controllers
{
    [Route("/api/[controller]")]
    public class CategoryController:ControllerBase
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

        [HttpPost]
        public Category Post([FromBody]Category category)
        { 
            return _categoryService.Add(category);
        }
        
        [HttpPut]
        public Category Put([FromBody]Category category)
        {
            return _categoryService.Update(category);
        }
    }
}
