using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WebApiMyLib.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<Autor> Autors { get; set; }
        public ICollection<Category> Categories { get; set; }

        public Book()
        {
            Autors = new List<Autor>();
            Categories = new List<Category>();
        }

    }
}
