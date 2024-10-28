using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace ISO710_BOOKS.Tests;

[TestFixture]
public class MiembroFormTests
{
    private EdgeDriver driver;
    private readonly string base_url = "https://localhost:7184/Miembros";

    [SetUp]
    public void Setup()
    {
        driver = new EdgeDriver();
        driver.Manage().Window.Maximize();
    }

    [Test]
    public void CreateMiembroTest()
    {
        driver.Navigate().GoToUrl($"{base_url}/Create");

        // Llena el campo "Nombre Completo"
        var nombreCompleto = driver.FindElement(By.Id("NombreCompleto"));
        nombreCompleto.SendKeys("Rafael Rodriguez");

        // Llena el campo "Correo"
        var correo = driver.FindElement(By.Id("Correo"));
        correo.SendKeys("r.rodriguez@example.com");

        // Llena el campo "Teléfono"
        var telefono = driver.FindElement(By.Id("Telefono"));
        telefono.SendKeys("(123) 456-7890");

        // Llena el campo "Dirección"
        var direccion = driver.FindElement(By.Id("Direccion"));
        direccion.SendKeys("123 Calle Falsa, Ciudad Ficticia");

        // Enviar el formulario
        var submitButton = driver.FindElement(By.CssSelector("button[type='submit']"));
        submitButton.Click();

        // Verificar que la URL sea la esperada después de la redirección
        Assert.That(driver.Url, Is.EqualTo(base_url),
            $"La redirección no fue exitosa. Se esperaba {base_url} pero se obtuvo {driver.Url}.");
    }

    [Test]
    public void CreateMiembro_InvalidEmailTest()
    {
        driver.Navigate().GoToUrl($"{base_url}/Create");

        var nombreCompleto = driver.FindElement(By.Id("NombreCompleto"));
        nombreCompleto.SendKeys("Juan Pérez");

        var correo = driver.FindElement(By.Id("Correo"));
        correo.SendKeys("correo_invalido");

        var submitButton = driver.FindElement(By.CssSelector("button[type='submit']"));
        submitButton.Click();

        // Verifica que el mensaje de error esté presente
        Assert.That(driver.PageSource, Does.Contain("Por favor ingresa un correo electrónico válido."), "El mensaje de validacion no fue encontrado en la página.");
    }

    [TearDown]
    public void TearDown()
    {
        Thread.Sleep(3000);
        driver.Quit();
        driver.Dispose();
    }
}
