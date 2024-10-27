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
            string categoria = ObtenerCategoriaAleatoria();
            List<LibroModel> libros = await booksService.ObtenerLibrosAsync(categoria);
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

        private string ObtenerCategoriaAleatoria()
        {
            var categorias = new List<string> { "history", "science", "technology", "art", "literature", "sports", "health", "philosophy", "music", "education" };
            Random random = new Random();
            int index = random.Next(categorias.Count);
            return categorias[index];
        }

    }
}
