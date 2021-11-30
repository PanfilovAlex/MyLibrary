using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiMyLib.Models
{
    public class BookDto
    {
        public string Title { get; set; }
        public ICollection<AutorDto> Autors { get; set; }
        public ICollection<CategoryDto> Categories { get; set; }
        public BookDto()
        {
            Autors = new List<AutorDto>();
            Categories = new List<CategoryDto>();
        }
    }
}
