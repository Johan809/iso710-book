using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ISO710_BOOKS.Models;

public partial class Miembro
{
    public int MiembroId { get; set; }

    [DisplayName("Nombre Completo")]
    public string NombreCompleto { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    [DisplayName("Fecha de Registro")]
    public DateTime? FechaRegistro { get; set; }

    public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
}
