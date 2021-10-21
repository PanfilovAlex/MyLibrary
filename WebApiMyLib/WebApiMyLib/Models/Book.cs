using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiMyLib.Models
{
    public class Book
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public List<string> Category { get; set; }
        public Autor Autor { get; set; }
    }
}
