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
        public IEnumerable<Category> Get() => _categoryService.Categories;

        [HttpGet("{id}")]
        public Category Get(int id) => _categoryService.Find(id);

        [HttpPost]
        public Category Post([FromBody]Category category) => _categoryService.Add(category);
    }
}
