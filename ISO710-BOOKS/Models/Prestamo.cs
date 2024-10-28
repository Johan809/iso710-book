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
