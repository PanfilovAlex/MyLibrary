using System.Collections.Generic;

namespace WebApiMyLib.Data.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public ICollection<Book> Books { get; set; }
        public bool IsDeleted { get; set; }
        public Author()
        {
            Books = new List<Book>();
        }
    }
}
