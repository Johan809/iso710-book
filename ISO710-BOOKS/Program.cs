using ISO710_BOOKS.Models;
using ISO710_BOOKS.Services;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Configurar cultura
var defaultCulture = new CultureInfo("es-ES");
CultureInfo.DefaultThreadCurrentCulture = defaultCulture;
CultureInfo.DefaultThreadCurrentUICulture = defaultCulture;

// Agregar servicios al contenedor
builder.Services.AddHttpClient<GoogleBooksService>();
builder.Services.Configure<GoogleBooksSettings>(builder.Configuration.GetSection("ApiSettings"));
builder.Services.AddControllersWithViews();
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", builder =>
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
builder.Services.AddControllers();

builder.Services.AddDbContext<Iso710Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

WebApplication app = builder.Build();

// Configuración del pipeline de solicitudes HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Permitir servir archivos estáticos
app.UseCors("PermitirTodo");
app.UseRouting();
app.UseAuthorization();

// **Servir archivos estáticos desde la carpeta ./app**
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "app")),
    RequestPath = ""
});

// **Ruta predeterminada redirigida a Angular**
app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();

    // Redireccionar cualquier otra ruta al index.html de Angular
    _ = endpoints.MapFallbackToFile("index.html", new StaticFileOptions
    {
        FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), "App/iso-books-ng/dist/iso-books-ng/browser"))
    });
});

// **Eliminar la ruta MVC predeterminada**
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
