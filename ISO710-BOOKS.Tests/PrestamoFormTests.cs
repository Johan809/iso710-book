using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace ISO710_BOOKS.Tests;

[TestFixture]
public class PrestamoFormTests
{
    private EdgeDriver driver;
    private readonly string base_url = "https://localhost:7184/Prestamos";

    [SetUp]
    public void Setup()
    {
        driver = new EdgeDriver();
        driver.Manage().Window.Maximize();
    }

    [Test]
    public void CreatePrestamoTest()
    {
        driver.Navigate().GoToUrl($"{base_url}/Create/7eDGBgAAQBAJ");

        // Selecciona un miembro en el selector de MiembroId
        var miembroSelect = driver.FindElement(By.Id("MiembroId"));
        var miembroOption = miembroSelect.FindElement(By.CssSelector("option[value='2']")); // Usar el ID real de un miembro de prueba
        miembroOption.Click();

        // Llena el campo ISBN
        var isbn = driver.FindElement(By.Id("isbn"));
        isbn.SendKeys("9781733139243");

        // Llena el campo Título
        var titulo = driver.FindElement(By.Id("Titulo"));
        titulo.SendKeys("Ejemplo de Título");

        // Llena el campo Fecha de Devolución
        var fechaDevolucion = driver.FindElement(By.Id("FechaDevolucion"));
        fechaDevolucion.SendKeys("12/31/2024");

        // Marcar opciones adicionales
        var esUrgente = driver.FindElement(By.Id("EsUrgente"));
        esUrgente.Click();

        var esEdicionEspecial = driver.FindElement(By.Id("EsEdicionEspecial"));
        esEdicionEspecial.Click();

        // Llena el campo Comentario
        var comentario = driver.FindElement(By.Id("Comentario"));
        comentario.SendKeys("Este es un comentario de prueba para el préstamo.");

        string libroIdValue = "7eDGBgAAQBAJ"; // Reemplaza este valor con el que necesites
        ((IJavaScriptExecutor)driver).ExecuteScript($"document.getElementsByName('LibroId')[0].value = '{libroIdValue}';");

        // Enviar el formulario
        var submitButton = driver.FindElement(By.Id("btnSubmit"));
        submitButton.Click();

        // Verificar que la redirección a la página de índice sea exitosa
        Assert.That(driver.Url, Is.EqualTo(base_url),
            $"La redirección no fue exitosa. Se esperaba {base_url} pero se obtuvo {driver.Url}.");
    }

    [Test]
    public void CreatePrestamo_EmptyFieldsTest()
    {
        driver.Navigate().GoToUrl($"{base_url}/Create");

        // Intenta enviar el formulario sin llenar campos obligatorios
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        var submitButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnSubmit")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
        submitButton.Click();

        // Verifica que aparezcan mensajes de validación para los campos requeridos
        wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(text(), 'El ISBN es obligatorio.')]")));

        Assert.That(driver.PageSource, Does.Contain("El ISBN es obligatorio."),
            "El mensaje de validación para ISBN no fue encontrado en la página.");

        wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(text(), 'El titulo del libro es obligatorio.')]")));
        Assert.That(driver.PageSource, Does.Contain("El titulo del libro es obligatorio."),
            "El mensaje de validación para el Título no fue encontrado en la página.");

        wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(text(), 'La fecha de devolucion es obligatoria.')]")));
        Assert.That(driver.PageSource, Does.Contain("La fecha de devolucion es obligatoria."),
            "El mensaje de validación para la Fecha de Devolución no fue encontrado en la página.");
    }

    [TearDown]
    public void TearDown()
    {
        //esperar 3 segs antes de cerrar todo;
        Thread.Sleep(3000);
        driver.Quit();
        driver.Dispose();
    }
}

