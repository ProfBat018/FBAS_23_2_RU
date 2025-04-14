# Тема урока: Minimal API

- Что такое API ?
- Что такое Minimal API ?
- OpenApi specification
- Swagger
- Postman
- Типы HTTP запросов

## Что такое API ?

`API` - Application programming interface, это набор методов которые позволяют нам выполнять операции над данными. В современной `Web` разработке `API` это набор `Endpoint` которые принимают и возвращают данные в формате `JSON` или `XML`. Если мы создали приложение типа `SPA` на React или `Mobile` на React Native то нам нужно создать `API` для взаимодействия с нашими данными.

## Что такое Minimal API ?

`Minimal API` - это подход к разработке API с помощью технологии `Top Level Statements`. То есть мы с вами пишем код сразу в `Main`. такой подход нужен для легких и маленьких проектов, где нет необходимости в большом количестве файлов и классов.

По идее это тоже самое что и `Python Fast Api` или `Node.js Express`.

Вам достаточно лишь создать обычный web api проект и не выбирать галочку `Do not use top level statements`.

## Пример Minimal API

```csharp
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



```

# OpenApi specification

Дело в том, что просто взять и писать API - это очень легко. Но есть такие организации как,
`ANSI`, `ISO`. Мы должны понимать что все должно быть стандартизировано. Если вы пишите `API`, он
должен быть описан в спецификации `OpenApi`.
Это позволяет другим разработчикам понимать какие `Endpoint` у вас есть и какие данные они принимают и возвращают.
Это своего рода документация к вашему `API`. У нас в `ASP.NET` есть сервис, который называется `AddEndpointsApiExplorer`.
Он позволяет нам генерировать `OpenApi` спецификацию на основе нашего кода.

# Swagger

`Swagger` - это инструмент для разработки API. С ним очень просто можно тестриовать ваши `Endpoint`-ы.
Также `Swagger` позволяет вам сгенерировать документацию к вашему `API`.

# Postman

`Postman` - это инструмент для тестирования `API`. Он позволяет вам отправлять запросы на ваш `API` и смотреть ответы. Мне он кажется более удобным и советую его вам скачать. В будущем мы будем использовать его для тестирования наших `API`.

# Типы HTTP запросов

`HTTP` поддерживает несколько типов запросов. Наиболее популярные это `GET`, `POST`, `PUT`, `DELETE`, остальные
можно сказать редко используются. В самом подходе `MinimalApi`, то есть когда мы пишем все в Main уже есть готовые
методы для их реализации. Например `MapGet`, `MapPost`, `MapPut`, `MapDelete`.
