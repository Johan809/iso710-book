using System;
using System.Collections.Generic;

namespace ISO710_BOOKS.Models;

public partial class Prestamo
{
    public int PrestamoId { get; set; }

    public int MiembroId { get; set; }

    public string Isbn { get; set; } = null!;

    public string Titulo { get; set; } = null!;

    public DateTime? FechaPrestamo { get; set; }

    public DateTime? FechaDevolucion { get; set; }

    public bool? Devuelto { get; set; }

    public virtual Miembro Miembro { get; set; } = null!;
}
