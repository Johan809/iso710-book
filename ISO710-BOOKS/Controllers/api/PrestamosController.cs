using ISO710_BOOKS.Models;
using ISO710_BOOKS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ISO710_BOOKS.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrestamosController : ControllerBase
    {
        private readonly Iso710Context _context;
        private readonly GoogleBooksService _googleBooksService;

        public PrestamosController(Iso710Context context, GoogleBooksService googleBooksService)
        {
            _context = context;
            _googleBooksService = googleBooksService;
        }

        // GET: api/Prestamos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prestamo>>> GetPrestamos()
        {
            return await _context.Prestamos.ToListAsync();
        }

        // GET: api/Prestamos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Prestamo>> GetPrestamo(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);

            if (prestamo == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(prestamo.LibroId))
            {
                prestamo.Libro = await _googleBooksService.ObtenerLibroPorId(prestamo.LibroId);
            }
            Miembro miembro = await _context.Miembros.FindAsync(prestamo.MiembroId);
            if (miembro != null)
                prestamo.Miembro = miembro;

            return prestamo;
        }

        // PUT: api/Prestamos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<string>> PutPrestamo(int id, Prestamo prestamo)
        {
            if (id != prestamo.PrestamoId)
            {
                return BadRequest();
            }
            Prestamo tempPrestamo = await _context.Prestamos.FindAsync(id) ?? new Prestamo();
            if (tempPrestamo == null)
            {
                return NotFound();
            }

            tempPrestamo.Estado = prestamo.Estado;
            tempPrestamo.EsUrgente = prestamo.EsUrgente;
            tempPrestamo.EsEdicionEspecial = prestamo.EsEdicionEspecial;
            tempPrestamo.FechaDevolucion = prestamo.FechaDevolucion;
            tempPrestamo.Devuelto = prestamo.Devuelto;
            tempPrestamo.Comentario = prestamo.Comentario;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrestamoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return "Actualizado con exito";
        }

        // POST: api/Prestamos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Prestamo>> PostPrestamo(Prestamo prestamo)
        {
            LibroModel? libro = null;
            if (!string.IsNullOrEmpty(prestamo.LibroId))
            {
                libro = await _googleBooksService.ObtenerLibroPorId(prestamo.LibroId);
            }
            else if (!string.IsNullOrEmpty(prestamo.Isbn))
            {
                libro = await _googleBooksService.ObtenerLibroPorISBN(prestamo.Isbn);
            }
            else if (!string.IsNullOrEmpty(prestamo.Titulo))
            {
                libro = await _googleBooksService.ObtenerLibroPorTitulo(prestamo.Titulo);
            }

            if (libro == null)
            {
                return BadRequest("El libro no pudo ser encontrado");
            }

            prestamo.LibroId = libro.Id;
            prestamo.Titulo = libro.Titulo;
            prestamo.Isbn = libro.ISBN;
            prestamo.FechaPrestamo = DateTime.Now;

            _context.Prestamos.Add(prestamo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPrestamo", new { id = prestamo.PrestamoId }, prestamo);
        }

        // DELETE: api/Prestamos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeletePrestamo(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo == null)
            {
                return NotFound();
            }

            _context.Prestamos.Remove(prestamo);
            await _context.SaveChangesAsync();

            return "Eliminado con exito";
        }

        private bool PrestamoExists(int id)
        {
            return _context.Prestamos.Any(e => e.PrestamoId == id);
        }
    }
}
