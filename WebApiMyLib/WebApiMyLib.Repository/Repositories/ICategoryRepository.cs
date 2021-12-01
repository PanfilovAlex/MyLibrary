using System;
using System.Collections.Generic;
using WebApiMyLib.Data.Models;

namespace WebApiMyLib.Repository.Repositories
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> Categories { get; }
        Category AddCategory(Category category);
        Category UpdateCategory(Category category);
        Category FindCategory(int id);
        void DeleteCategory(int id);

        int CheckCategory(Category category);

    }
}
