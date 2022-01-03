using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiMyLib.BLL.Interfaces;
using WebApiMyLib.Data.Models;
using WebApiMyLib.Data.Repositories;

namespace WebApiMyLib.BLL.Servicies
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private ICategoryValidationService _categoryValidationService;
        private CategoryExceptions exceptions;
        public CategoryService(ICategoryRepository categoryRepository, ICategoryValidationService categoryValidationService)
        {
            _categoryRepository = categoryRepository;
            _categoryValidationService = categoryValidationService;
        }
        public IEnumerable<Category> Categories => _categoryRepository.Categories.ToList();

        public Category Add(Category category)
        {
            
            if (!_categoryValidationService.Validate(category).IsValid)
            {
                exceptions = new CategoryExceptions(_categoryValidationService.Validate(category));
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
            
            if (!_categoryValidationService.Validate(category).IsValid)
            {
                exceptions = new CategoryExceptions(_categoryValidationService.Validate(category));
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
