using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiMyLib.Services
{
    public interface ISearchString<T>
    {
        IQueryable<T> SearchString(IQueryable<T> books, string searchString);
    }
}
