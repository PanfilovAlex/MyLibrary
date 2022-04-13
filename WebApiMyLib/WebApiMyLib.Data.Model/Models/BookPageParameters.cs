using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiMyLib.Data.Models
{
    public class BookPageParameters : RequestParameters
    {
        public string SearchString { get; set; }
        public string SortBy { get; set; }
    }
}
