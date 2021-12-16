using System;
using System.Collections.Generic;
using WebApiMyLib.Data.Models;
using WebApiMyLib.Data.Repositories;

namespace WebApiMyLib.BLL.Interfaces
{
    internal interface ICategoryService
    {
        IEnumerable<Category> Categories { get; }
        Category Add(Category category);
        Category Update(Category category);
        Category Find(int id);
        void Delete(int id);
    }
}
