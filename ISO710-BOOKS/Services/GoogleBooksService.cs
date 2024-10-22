using ISO710_BOOKS.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ISO710_BOOKS.Services;

public class GoogleBooksSettings
{
    public required string ApiKey { get; set; }
}

public class GoogleBooksService
{
    private readonly HttpClient httpClient;
    private readonly GoogleBooksSettings settings;
    private const string ApiUrl = "https://www.googleapis.com/books/v1/volumes?q=";

    public GoogleBooksService(HttpClient _httpClient, IOptions<GoogleBooksSettings> _settings)
    {
        httpClient = _httpClient;
        settings = _settings.Value;

        //headers
        httpClient.DefaultRequestHeaders.Clear();
        httpClient.DefaultRequestHeaders
            .Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.DefaultRequestHeaders
            .Authorization = new AuthenticationHeaderValue("key", "=" + settings.ApiKey);
    }

    public async Task<List<LibroModel>> ObtenerLibrosAsync(string query)
    {
        var response = await httpClient.GetAsync(ApiUrl + query);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var jsonResponse = JsonDocument.Parse(content);
        var libros = new List<LibroModel>();

        foreach (var item in jsonResponse.RootElement.GetProperty("items").EnumerateArray())
        {
            var volumeInfo = item.GetProperty("volumeInfo");
            string portada = string.Empty;
            if (volumeInfo.TryGetProperty("imageLinks", out var imageLinks))
            {
                portada = imageLinks.GetPropertyExtension("medium")?.GetString()
                    ?? imageLinks.GetPropertyExtension("small")?.GetString()
                    ?? imageLinks.GetPropertyExtension("thumbnail")?.GetString()
                    ?? string.Empty;
            }

            libros.Add(new LibroModel
            {
                Titulo = volumeInfo.TryGetProperty("title", out JsonElement title)
                    ? title.GetString() : "Título desconocido",
                Autor = volumeInfo.TryGetProperty("authors", out var authors)
                    ? string.Join(", ", authors.EnumerateArray().Select(a => a.GetString() ?? "Desconocido"))
                    : "Desconocido",
                ISBN = volumeInfo.TryGetProperty("industryIdentifiers", out var identifiers) && identifiers.GetArrayLength() > 0
                    ? identifiers[0].GetProperty("identifier").GetString() ?? "N/A"
                    : "N/A",
                Editorial = volumeInfo.TryGetProperty("publisher", out var publisher)
                    ? publisher.GetString() : "Editorial desconocida",
                AñoPublicacion = volumeInfo.TryGetProperty("publishedDate", out var publishedDate)
                     && int.TryParse(publishedDate.GetString().Substring(0, 4), out var year)
                     ? year
                     : 0,
                Descripcion = volumeInfo.TryGetProperty("description", out var description)
                    ? description.GetString() ?? "Sin descripción" : "Sin descripción",
                PortadaUrl = portada
            });

        }

        return libros;
    }

    public async Task<LibroModel?> ObtenerLibroPorISBN(string isbn)
    {
        var response = await httpClient.GetAsync(ApiUrl + "isbn:" + isbn);
        if (!response.IsSuccessStatusCode) return null;

        var content = await response.Content.ReadAsStringAsync();
        var jsonResponse = JsonDocument.Parse(content);

        if (jsonResponse.RootElement.TryGetProperty("totalItems", out var totalItems) && totalItems.GetInt32() > 0)
        {
            var volumeInfo = jsonResponse.RootElement.GetProperty("items")[0].GetProperty("volumeInfo");
            string portada = string.Empty;

            if (volumeInfo.TryGetProperty("imageLinks", out var imageLinks))
            {
                portada = imageLinks.GetPropertyExtension("medium")?.GetString()
                    ?? imageLinks.GetPropertyExtension("small")?.GetString()
                    ?? imageLinks.GetPropertyExtension("thumbnail")?.GetString()
                    ?? string.Empty;
            }

            return new LibroModel
            {
                Titulo = volumeInfo.TryGetProperty("title", out var title)
               ? title.GetString() ?? "Título desconocido" : "Título desconocido",
                Autor = volumeInfo.TryGetProperty("authors", out var authors)
               ? string.Join(", ", authors.EnumerateArray().Select(a => a.GetString() ?? "Desconocido"))
               : "Desconocido",
                ISBN = isbn,
                Editorial = volumeInfo.TryGetProperty("publisher", out var publisher)
               ? publisher.GetString() ?? "Editorial desconocida" : "Editorial desconocida",
                AñoPublicacion = volumeInfo.TryGetProperty("publishedDate", out var publishedDate)
               && int.TryParse(publishedDate.GetString().Substring(0, 4), out var year)
               ? year : 0,
                Descripcion = volumeInfo.TryGetProperty("description", out var description)
               ? description.GetString() ?? "Sin descripción" : "Sin descripción",
                PortadaUrl = portada
            };
        }

        return null;
    }
}
