using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApiMyLib.Data.Models
{
    public class Autor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите имя автора")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Имя должно содержать не менее 2-х и не более 50 символов")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Введите фамилию автора")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Фамилия должно содержать не менее 2-х и не более 50 символов")]
        public string LastName { get; set; }

        public ICollection<Book> Books { get; set; }
        public bool IsDeleted { get; set; }

        public Autor()
        {
            Books = new List<Book>();
        }
    }
}
