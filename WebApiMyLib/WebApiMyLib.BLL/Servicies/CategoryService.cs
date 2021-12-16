﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApiMyLib.BLL.Interfaces;
using WebApiMyLib.Data.Models;
using WebApiMyLib.Data.Repositories;

namespace WebApiMyLib.BLL.Servicies
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private IValidationDictionary _modelState;

        public CategoryService(ICategoryRepository categoryRepository, IValidationDictionary modelState)
        {
            _categoryRepository = categoryRepository;
            _modelState = modelState;
        }
        public IEnumerable<Category> Categories => _categoryRepository.Categories.ToList();

        public Category Add(Category category)
        {
            if(!CategoryValidationService.IsValid(_modelState, category))
            {
                return null;
            }

            try
            {
                _categoryRepository.Add(category);
            }
            catch
            {
                return null;
            }
            return _categoryRepository.Categories.FirstOrDefault(a => a.Name.Equals(category.Name));
        }
        
        public void Delete(int id)
        {
            var categoryToDelete = _categoryRepository.Categories.FirstOrDefault(a => a.Id == id);
            if(categoryToDelete == null)
            {
                return;
            }
            _categoryRepository.Delete(id);
        }

        public Category Find(int id)
        {
            var foundCatgory = _categoryRepository.Find(id);
            if (foundCatgory == null)
            { 
                return null; 
            }
            return foundCatgory;
        }

        public Category Update(Category category)
        {
            if (!CategoryValidationService.IsValid(_modelState, category))
            {
                return null;
            }

            var categoryToUpdate = _categoryRepository.Find(category.Id);
            if(categoryToUpdate == null)
            {
                return null;
            }
            categoryToUpdate = _categoryRepository.Update(category);

            return categoryToUpdate;
        }

        protected bool ValidateCategory(Category categoryToValidate)
        {
            if (categoryToValidate.Name.Trim().Length == 0)
            {
                _modelState.AddModelError("Name", "Name is requered!");
            }
            return _modelState.IsValid;
        }

        protected bool IsExist(Category category)
        {
            if (_categoryRepository.Categories.Any(a => a.Name.Equals(category.Name)))
            {
                return true;
            }
            return false;
        }
    }
}