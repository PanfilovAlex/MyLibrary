using Microsoft.AspNetCore.Mvc;
using WebApiMyLib.Data.Repositories;
using WebApiMyLib.Data.Models;

namespace WebApiMyLib.Controllers
{
    public class HomeController:Controller
    {
        private IBookRepository _repository { get; set; }

        public HomeController(IBookRepository bookRepository) => _repository = bookRepository;
        public ViewResult Index() => View(_repository.GetBooks);
        [HttpPost]
        public IActionResult AddBook(Book book)
        {
            _repository.AddBook(book);
            return RedirectToAction("Index");
        }



    }
}
