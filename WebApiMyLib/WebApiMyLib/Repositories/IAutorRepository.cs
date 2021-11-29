using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiMyLib.Models;

namespace WebApiMyLib.Repositories  
{
    public interface IAutorRepository
    {
        IEnumerable<Autor> GetAutors { get; }
        IEnumerable<Autor> Autors(PageParameters pageParameters);
        public Autor Find(int id);
        public Autor Add(Autor autor);
        public Autor Update(Autor autor);
        public void Delete(int id);


    }
}
