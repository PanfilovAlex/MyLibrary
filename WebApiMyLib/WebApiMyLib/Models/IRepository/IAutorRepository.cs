﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiMyLib.Models.IRepository
{
    public interface IAutorRepository
    {
        IEnumerable<Autor> Autors { get; }
        public Autor Find(int id);
        public Autor Add(Autor autor);
        public Autor Update(Autor autor);
        public void Delete(int id);


    }
}
