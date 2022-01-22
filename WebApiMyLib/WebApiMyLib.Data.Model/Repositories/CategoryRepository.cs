using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WebApiMyLib.Data.Models;

namespace WebApiMyLib.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private BookDbContext _repository;

        public CategoryRepository(BookDbContext context) => _repository = context;
        
        public IEnumerable<Category> Categories => _repository.Categories;

        public IEnumerable<Category> GetCategories(Expression<Func<Category, bool>> expression)
        {
            return _repository.Categories.Where(expression).ToList();
        }

        public Category Add(Category category)
        {
            var newCategory = new Category
            {
                Name = category.Name,
            };

            _repository.Categories.Add(newCategory);
            _repository.SaveChanges();
            return newCategory;
        }

        public void Delete(int id)
        {
            var deletedCategory = _repository.Categories.FirstOrDefault(c => c.Id == id);
            deletedCategory.IsDeleted = true;
            _repository.SaveChanges();
        }

        public Category Find(int id)
        {
            var foundRepicpe = _repository.Categories.FirstOrDefault(a => a.Id == id);

            return foundRepicpe;
        }

        public Category Update(Category category)
        {
            var updatedCategory = _repository.Categories.FirstOrDefault(c => c.Id == category.Id);
            updatedCategory.Name = category.Name;
            updatedCategory.IsDeleted = category.IsDeleted;

            _repository.SaveChanges();

            return updatedCategory;
        }
    }
}
