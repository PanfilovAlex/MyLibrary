using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiMyLib.Models;

namespace WebApiMyLib.Controllers
{
    public class HomeController:Controller
    {
        private IBookRepository _repository { get; set; }

        public HomeController(IBookRepository bookRepository) => _repository = bookRepository;
        public ViewResult Index() => View(_repository.Books);
        [HttpPost]
        public IActionResult AddBook(Book book)
        {
            _repository.AddBook(book);
            return RedirectToAction("Index");
        }
    }
}
