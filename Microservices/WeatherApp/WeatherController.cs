using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WeatherApp;

[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
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

        return Ok(weatherData);
    }
    
}