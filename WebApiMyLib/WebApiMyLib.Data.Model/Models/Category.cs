using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApiMyLib.Data.Models
{
    public class Category
    {
        public int Id { get; set; }

        //[Required(ErrorMessage = "Введите название категории")]
        public string Name { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public ICollection<Book> Books { get; set; }

        public Category()
        {
            Books = new List<Book>();
        }
    }
}
