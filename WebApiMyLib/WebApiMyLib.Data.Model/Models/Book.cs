using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApiMyLib.Data.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Введите название книги")]
        public string Title { get; set; }
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
