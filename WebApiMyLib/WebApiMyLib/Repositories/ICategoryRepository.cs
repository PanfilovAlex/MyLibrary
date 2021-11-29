using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiMyLib.Models.IRepository
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
