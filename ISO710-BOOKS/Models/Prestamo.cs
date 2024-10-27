using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ISO710_BOOKS.Models;

public partial class Prestamo
{
    public int PrestamoId { get; set; }

    [DisplayName("Miembro")]
    public int MiembroId { get; set; }

    [DisplayName("ISBN")]
    [Required(ErrorMessage = "El ISBN es obligatorio.")]
    public string Isbn { get; set; } = null!;

    [DisplayName("Título")]
    [Required(ErrorMessage = "El titulo del libro es obligatorio.")]
    [StringLength(255, ErrorMessage = "El titulo no puede tener más de 255 caracteres")]
    public string Titulo { get; set; } = null!;

    [DisplayName("Fecha Préstamo")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
    public DateTime? FechaPrestamo { get; set; } = DateTime.Now;

    [DisplayName("Fecha Devolución")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
    [Required(ErrorMessage = "La fecha de devolucion es obligatoria.")]
    public DateTime? FechaDevolucion { get; set; } = DateTime.Now.AddMonths(1);

    [JsonIgnore]
    public bool? Devuelto { get; set; } = false;

    public string? Comentario { get; set; }

    // Campos adicionales
    [DisplayName("Estado")]
    public string Estado { get; set; } = "Activo";
    [DisplayName("Es Urgente")]

    public bool EsUrgente { get; set; } = false;

    [DisplayName("Edición Especial")]
    public bool EsEdicionEspecial { get; set; } = false;

    [JsonIgnore]
    public string? LibroId { get; set; }

    public virtual Miembro? Miembro { get; set; } = null!;
    public virtual LibroModel? Libro { get; set; } = null!;
}


public static class PrestamoEndpoints
{
    public static void MapPrestamoEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Prestamo").WithTags(nameof(Prestamo));

        group.MapGet("/", async (Iso710Context db) =>
        {
            return await db.Prestamos.ToListAsync();
        })
        .WithName("GetAllPrestamos")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Prestamo>, NotFound>> (int prestamoid, Iso710Context db) =>
        {
            return await db.Prestamos.AsNoTracking()
                .FirstOrDefaultAsync(model => model.PrestamoId == prestamoid)
                is Prestamo model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetPrestamoById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int prestamoid, Prestamo prestamo, Iso710Context db) =>
        {
            var affected = await db.Prestamos
                .Where(model => model.PrestamoId == prestamoid)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.PrestamoId, prestamo.PrestamoId)
                  .SetProperty(m => m.MiembroId, prestamo.MiembroId)
                  .SetProperty(m => m.Isbn, prestamo.Isbn)
                  .SetProperty(m => m.Titulo, prestamo.Titulo)
                  .SetProperty(m => m.FechaPrestamo, prestamo.FechaPrestamo)
                  .SetProperty(m => m.FechaDevolucion, prestamo.FechaDevolucion)
                  .SetProperty(m => m.Devuelto, prestamo.Devuelto)
                  .SetProperty(m => m.Comentario, prestamo.Comentario)
                  .SetProperty(m => m.Estado, prestamo.Estado)
                  .SetProperty(m => m.EsUrgente, prestamo.EsUrgente)
                  .SetProperty(m => m.EsEdicionEspecial, prestamo.EsEdicionEspecial)
                  .SetProperty(m => m.LibroId, prestamo.LibroId)
                  );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdatePrestamo")
        .WithOpenApi();

        group.MapPost("/", async (Prestamo prestamo, Iso710Context db) =>
        {
            db.Prestamos.Add(prestamo);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Prestamo/{prestamo.PrestamoId}", prestamo);
        })
        .WithName("CreatePrestamo")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int prestamoid, Iso710Context db) =>
        {
            var affected = await db.Prestamos
                .Where(model => model.PrestamoId == prestamoid)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeletePrestamo")
        .WithOpenApi();
    }
}