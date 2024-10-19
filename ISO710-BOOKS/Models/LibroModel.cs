namespace ISO710_BOOKS.Models;

public class LibroModel
{
    public required string Titulo { get; set; }
    public required string Autor { get; set; }
    public required string ISBN { get; set; }
    public required string Editorial { get; set; }
    public required int AñoPublicacion { get; set; }
    public required string Descripcion { get; set; }
    public required string PortadaUrl { get; set; }
}
