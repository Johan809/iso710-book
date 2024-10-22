using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ISO710_BOOKS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        // Ejemplo: Obtener una lista de productos
        [HttpGet]
        public IActionResult GetProductos()
        {
            var productos = new List<string> { "Producto1", "Producto2", "Producto3" };
            return Ok(productos); // Devuelve el resultado en formato JSON
        }

        // Ejemplo: Obtener un producto por su id
        [HttpGet("{id}")]
        public IActionResult GetProducto(int id)
        {
            var producto = $"Producto {id}";
            return Ok(producto);
        }
    }
}
