using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WebApiMyLib.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Autor { get; set; }
        public bool IsDeleted { get; set; }

    }
}
