using System;
using System.Collections.Generic;
using System.Linq;
using WebApiMyLib.Data.Models;

namespace WebApiMyLib.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private BookDbContext _repository;

        public CategoryRepository(BookDbContext context) => _repository = context;
        public IEnumerable<Category> Categories => _repository.Categories; 

        public Category Add(Category category)
        {
            var newCategory = new Category
            {
                Id = category.Id,
                Name = category.Name,
                Books = category.Books?.Select(b => new Book
                {
                    Id = b.Id,
                    Title = b.Title,
                    IsDeleted = b.IsDeleted,
      
                }).ToList()
            };
            
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
            updatedCategory.IsChosen = category.IsChosen;
            updatedCategory.IsDeleted = category.IsDeleted;

            _repository.SaveChanges();

            return updatedCategory;
        }
    }
}
