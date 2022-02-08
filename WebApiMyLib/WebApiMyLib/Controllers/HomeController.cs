using Microsoft.AspNetCore.Mvc;
using WebApiMyLib.Data.Models;
using WebApiMyLib.BLL.Interfaces;

namespace WebApiMyLib.Controllers
{
    public class HomeController:Controller
    {
        private IBookService _bookService { get; set; }

        public HomeController(IBookService service) => _bookService = service;
        public ViewResult Index() => View(_bookService.GetBooks);
        [HttpPost]
        public IActionResult AddBook(Book book)
        {
            _bookService.Add(book);
            return RedirectToAction("Index");
        }
    }
}
