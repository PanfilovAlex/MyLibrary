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
        public IEnumerable<Category> Categories => _categoryRepository.Categories.ToList();

        public Category Add(Category category)
        {
            var validationResult = _categoryValidationService.Validate(category);
            if (!validationResult.IsValid)
            {
                var validator = validationResult.Errors;
                throw new ValidationException(validationResult);
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
                return null;
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

            var categoryToUpdate = _categoryRepository.Find(category.Id);
            if (categoryToUpdate == null)
            {
                return null;
            }
            categoryToUpdate = _categoryRepository.Update(category);

            return categoryToUpdate;
        }
    }
}
