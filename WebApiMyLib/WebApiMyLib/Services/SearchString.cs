using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiMyLib.Services
{
    public class SearchString<T> : ISearchString<T>
    {
        IQueryable<T> ISearchString<T>.SearchString(IQueryable<T> books, string searchString)
        {
            throw new NotImplementedException();
        }
    }
}
