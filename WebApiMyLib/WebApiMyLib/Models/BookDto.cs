using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiMyLib.Models
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<AuthorDto> Authors { get; set; }
        public ICollection<CategoryDto> Categories { get; set; }
        public BookDto()
        {
            Authors = new List<AuthorDto>();
            Categories = new List<CategoryDto>();
        }
    }
}
