using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using WebApiMyLib.Data.Models;

namespace WebApiMyLib.Data.Repositories
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetCategories (Expression<Func<Category, bool>> predicate);
        IEnumerable<Category> Categories { get; }
        Category Add(Category category);
        Category Update(Category category);
        Category Find(int id);
        void Delete(int id);
    }
}
