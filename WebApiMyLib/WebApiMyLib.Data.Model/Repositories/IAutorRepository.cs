﻿using System.Collections.Generic;
using WebApiMyLib.Data.Models;

namespace WebApiMyLib.Data.Repositories
{
    public interface IAutorRepository
    {
        IEnumerable<Author> GetAutors { get; }
        PagedList<Author> Autors(PageParameters pageParameters);
        public Author Find(int id);
        public Author Add(Author autor);
        public Author Update(Author autor);
        public void Delete(int id);
    }
}
