using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiMyLib.Models;

namespace WebApiMyLib.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private BookDbContext _repository;

        public CategoryRepository(BookDbContext context) => _repository = context;
        public IEnumerable<Category> Categories => _repository.Categories; 

        public Category AddCategory(Category category)
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

        public void DeleteCategory(int id)
        {
            var deletedCategory = _repository.Categories.FirstOrDefault(c => c.Id == id);
            deletedCategory.IsDeleted = true;
            _repository.SaveChanges();
        }

        public Category FindCategory(int id)
        {
            var foundRepicpe = _repository.Categories.FirstOrDefault(a => a.Id == id);
                
            return foundRepicpe;
        }

        public Category UpdateCategory(Category category)
        {
            var updatedCategory = _repository.Categories.FirstOrDefault(c => c.Id == category.Id);
            if(updatedCategory == null)
            {
                throw new Exception("Category was not found");
            }
            updatedCategory.Name = category.Name;
            updatedCategory.IsChosen = category.IsChosen;
            updatedCategory.IsDeleted = category.IsDeleted;
            _repository.SaveChanges();
            return updatedCategory;
        }
        public int CheckCategory(Category category)
        {
            var checkedCategory = _repository.Categories
                .FirstOrDefault(c =>
                c.Name.Contains(category.Name, StringComparison.InvariantCultureIgnoreCase));
            return checkedCategory.Id;
        }
    }
}
