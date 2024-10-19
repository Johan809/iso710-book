using ISO710_BOOKS.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

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
                PortadaUrl = volumeInfo.TryGetProperty("imageLinks", out var imageLinks)
                  ? imageLinks.GetProperty("thumbnail").GetString() ?? null
                  : null
            });

        }

        return libros;
    }
}
