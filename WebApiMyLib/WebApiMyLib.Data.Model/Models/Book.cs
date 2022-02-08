using System.Collections.Generic;

namespace WebApiMyLib.Data.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public ICollection<Author> Authors { get; set; }
        public ICollection<Category> Categories { get; set; }
        public Book()
        {
            Authors = new List<Author>();
            Categories = new List<Category>();
        }
    }
}
