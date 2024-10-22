using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ISO710_BOOKS.Models;

public partial class Prestamo
{
    public int PrestamoId { get; set; }

    [DisplayName("Miembro")]
    public int MiembroId { get; set; }

    [DisplayName("ISBN")]
    [Required(ErrorMessage = "El nombre completo es obligatorio.")]
    public string Isbn { get; set; } = null!;

    [Required(ErrorMessage = "El titulo del libro es obligatorio.")]
    [StringLength(255, ErrorMessage = "El titulo no puede tener más de 255 caracteres")]
    public string Titulo { get; set; } = null!;

    [DisplayName("Fecha Prestamo")]
    public DateTime? FechaPrestamo { get; set; } = DateTime.Now;

    [DisplayName("Fecha Devolución")]
    public DateTime? FechaDevolucion { get; set; } = DateTime.Now.AddMonths(1);

    public bool? Devuelto { get; set; }

    public string? Comentario { get; set; }

    public virtual Miembro Miembro { get; set; } = null!;
}
