using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApiMyLib.Models
{
    public class SearchStringPageParameters : PageParameters
    {
        public string searchString { get; set; }

        public static IEnumerable<Book> SearchString(IQueryable<Book> books, string searchString)
        {
            if (!books.Any() || string.IsNullOrWhiteSpace(searchString))
            {
                return null;
            }

            var booksByTitle = books.Where(a => a.Title.ToLower().Contains(searchString.ToLower()));
            var booksByLastNameAutor = books.Include(a => a.Autors.Where(b => b.LastName.ToLower().Contains(searchString.ToLower())));
            var booksByFirstNameAutor = books.Include(a => a.Autors.Where(b => b.FirstName.ToLower().Contains(searchString.ToLower())));
            var searchBooks = booksByTitle
                .Concat(booksByFirstNameAutor)
                .Concat(booksByLastNameAutor)
                .ToList();
            return searchBooks;
        }
    }
}
