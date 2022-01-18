using System;
using System.Collections.Generic;
using System.Linq;
using WebApiMyLib.BLL.Interfaces;
using WebApiMyLib.Data.Models;
using WebApiMyLib.Data.Repositories;

namespace WebApiMyLib.BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private IValidationService<Category> _categoryValidationService;

        public CategoryService(ICategoryRepository categoryRepository, IValidationService<Category> categoryValidationService)
        {
            _categoryRepository = categoryRepository;
            _categoryValidationService = categoryValidationService;
        }
        public IEnumerable<Category> Categories => 
            _categoryRepository
            .GetCategories(x => x.IsDeleted == false);

        public Category Add(Category category)
        {
            var addedCategory = new Category();
            var validationResult = _categoryValidationService.Validate(category);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }

            try
            {
               return _categoryRepository.Add(category);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public void Delete(int id)
        {
            var categoryToDelete = _categoryRepository.Categories.FirstOrDefault(a => a.Id == id);
            if (categoryToDelete == null)
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
                throw new Exception("Category wasn't found");
            }
            return foundCatgory;
        }

        public Category Update(Category category)
        {
            var validationResult = _categoryValidationService.Validate(category);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }

            try
            {
                return _categoryRepository.Update(category);
            }
            catch
            {
                throw new Exception("Category was not found");
            }
        }
    }
}
