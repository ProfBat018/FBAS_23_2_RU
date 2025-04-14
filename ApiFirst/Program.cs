using Microsoft.AspNetCore.Builder.Extensions;

var builder = WebApplication.CreateBuilder(args); // Создает builder для приложения

// Serives- это IServiceCollection, который содержит все сервисы, которые используются в приложении

builder.Services.AddEndpointsApiExplorer(); // Добавляет сервисы для генерации OpenAPI
builder.Services.AddSwaggerGen(); // Добавляет Swagger, для документации API


// Создает приложение, которое вовзращает объект WebApplication
var app = builder.Build(); 


if (app.Environment.IsDevelopment()) // Если приложение находится в режиме разработки
{
    app.UseSwagger(); // Использует Swagger
    app.UseSwaggerUI(); // Использует SwaggerUI
}


app.UseHttpsRedirection(); // Использует перенаправление на https

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}



