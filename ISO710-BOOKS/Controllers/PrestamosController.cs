using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ISO710_BOOKS.Models;
using ISO710_BOOKS.Services;

namespace ISO710_BOOKS.Controllers
{
    public class PrestamosController : Controller
    {
        private readonly Iso710Context _context;
        private readonly GoogleBooksService _googleBooksService;

        public PrestamosController(Iso710Context context, GoogleBooksService googleBooksService)
        {
            _context = context;
            _googleBooksService = googleBooksService;
        }

        // GET: Prestamos
        public async Task<IActionResult> Index()
        {
            var iso710Context = _context.Prestamos.Include(p => p.Miembro);
            return View(await iso710Context.ToListAsync());
        }

        // GET: Prestamos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamos
                .Include(p => p.Miembro)
                .FirstOrDefaultAsync(m => m.PrestamoId == id);
            if (prestamo == null)
            {
                return NotFound();
            }

            return View(prestamo);
        }

        //// GET: Prestamos/Create
        //public IActionResult Create()
        //{
        //    ViewData["MiembroId"] = new SelectList(_context.Miembros, "MiembroId", "NombreCompleto");
        //    return View();
        //}

        public async Task<IActionResult> Create(string? isbn)
        {
            // Si se proporciona un ISBN, intentamos obtener el libro de la API de Google Books
            if (!string.IsNullOrEmpty(isbn))
            {
                var libro = await _googleBooksService.ObtenerLibroPorISBN(isbn);
                if (libro != null)
                {
                    ViewBag.Libro = libro;
                }
                else
                {
                    ViewBag.Error = "No se encontró ningún libro con ese ISBN.";
                }
            }

            ViewData["MiembroId"] = new SelectList(_context.Miembros, "MiembroId", "NombreCompleto");
            return View();
        }

        // POST: Prestamos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PrestamoId,MiembroId,Isbn,Titulo,FechaPrestamo,FechaDevolucion,Devuelto,Comentario")] Prestamo prestamo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(prestamo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "MiembroId", "NombreCompleto", prestamo.MiembroId);
            return View(prestamo);
        }

        // GET: Prestamos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo == null)
            {
                return NotFound();
            }
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "MiembroId", "NombreCompleto", prestamo.MiembroId);
            return View(prestamo);
        }

        // POST: Prestamos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PrestamoId,MiembroId,Isbn,Titulo,FechaPrestamo,FechaDevolucion,Devuelto,Comentario")] Prestamo prestamo)
        {
            if (id != prestamo.PrestamoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prestamo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrestamoExists(prestamo.PrestamoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "MiembroId", "NombreCompleto", prestamo.MiembroId);
            return View(prestamo);
        }

        // GET: Prestamos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamos
                .Include(p => p.Miembro)
                .FirstOrDefaultAsync(m => m.PrestamoId == id);
            if (prestamo == null)
            {
                return NotFound();
            }

            return View(prestamo);
        }

        // POST: Prestamos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo != null)
            {
                _context.Prestamos.Remove(prestamo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Prestamos/ObtenerLibro
        public async Task<IActionResult> ObtenerLibro(string isbn)
        {
            if (string.IsNullOrEmpty(isbn))
            {
                return RedirectToAction(nameof(Create));
            }

            // Llama al servicio para obtener el libro por ISBN
            var libro = await _googleBooksService.ObtenerLibroPorISBN(isbn);

            if (libro != null)
            {
                ViewBag.Libro = libro;
            }
            else
            {
                ViewBag.Error = "No se encontró ningún libro con ese ISBN.";
            }

            ViewData["MiembroId"] = new SelectList(_context.Miembros, "MiembroId", "NombreCompleto");
            return View("Create"); // Recarga la vista de creación
        }

        private bool PrestamoExists(int id)
        {
            return _context.Prestamos.Any(e => e.PrestamoId == id);
        }
    }
}
