﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebApiMyLib.Repositories;
using WebApiMyLib.Models;

namespace WebApiMyLib.Controllers
{
    [Route("/api/[controller]")]
    public class CategoryController:ControllerBase
    {
        private ICategoryRepository _bdCategories;
        public CategoryController(ICategoryRepository repository)
        {
            _bdCategories = repository;
        }

        [HttpGet]
        public IEnumerable<Category> Get() => _bdCategories.Categories;

        [HttpGet("{id}")]
        public Category Get(int id) => _bdCategories.FindCategory(id);

        [HttpPost]
        public Category Post([FromBody]Category category) => _bdCategories.AddCategory(category);




    }
}
