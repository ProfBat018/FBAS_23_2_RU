# Введение в ApiController

`ApiController` - это класс, который наследуется от `ControllerBase` и предназначен для обработки запросов к API. Он возвращает тип `IActionResult`, который представляет собой абстрактный результат выполнения вашего кода. Его мы тоже рассмотрим унлубленно, так как я реализовал его с нуля.

Вот пример `ApiController`:

```csharp

// Тег, который позволяет вашей сборке понять что этот класс является контроллером.
[ApiController]
[Route("api/[controller]")] // Атрибут, который позволяет указать путь к контроллеру. Например http://localhost:5000/api/auth/login
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login()
    {
        return Ok("Login Success");
    }

    public void foo()
    {
        Console.WriteLine("foo");
    }
}

```
