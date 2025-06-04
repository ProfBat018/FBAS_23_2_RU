using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace WeatherApp;

[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    private readonly RabbitMqService _rabbit;

    public WeatherController(RabbitMqService rabbit)
    {
        _rabbit = rabbit;
    }

    [Authorize(Policy = "UserPolicy")]
    [HttpGet]
    public IActionResult GetWeather()
    {
        var weatherData = new
        {
            Temperature = "22°C",
            Condition = "Sunny",
            Location = "New York"
        };

        var json = JsonSerializer.Serialize(weatherData);
        _rabbit.Publish(json);
        return Ok(weatherData);
    }
    
}