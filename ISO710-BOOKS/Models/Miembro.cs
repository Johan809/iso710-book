using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ISO710_BOOKS.Models;

public partial class Miembro
{
    public int MiembroId { get; set; }

    [DisplayName("Nombre Completo")]
    [Required(ErrorMessage = "El nombre completo es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre completo no puede tener más de 100 caracteres.")]
    public string NombreCompleto { get; set; } = null!;

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "Por favor ingresa un correo electrónico válido.")]
    public string Correo { get; set; } = null!;

    [Phone(ErrorMessage = "El formato del teléfono no es válido.")]
    [StringLength(14, MinimumLength = 14, ErrorMessage = "El teléfono debe tener el formato (###) ###-####")]
    [RegularExpression(@"\(\d{3}\) \d{3}-\d{4}", ErrorMessage = "El formato del teléfono debe ser (###) ###-####")]
    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    [DisplayName("Fecha de Registro")]
    [DataType(DataType.DateTime)]
    public DateTime? FechaRegistro { get; set; }

    public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
}
