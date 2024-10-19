using ISO710_BOOKS.Models;
using ISO710_BOOKS.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ISO710_BOOKS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GoogleBooksService booksService;

        public HomeController(ILogger<HomeController> logger, GoogleBooksService _googleBooksService)
        {
            _logger = logger;
            booksService = _googleBooksService;
        }

        public async  Task<IActionResult> Index()
        {
            List<LibroModel> libros = await booksService.ObtenerLibrosAsync("programming");
            return View(libros);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
