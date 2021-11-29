using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApiMyLib.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Введите название категории")]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsChosen { get; set; }

        public ICollection<Book> Books { get; set; }

        public Category()
        {
            Books = new List<Book>();
        }
    }
}
