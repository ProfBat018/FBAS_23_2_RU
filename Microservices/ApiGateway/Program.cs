using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80); // Слушаем порт 80
});

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

builder.Services.AddOcelot();

var app = builder.Build();

// Добавим тестовый endpoint ДО Ocelot
app.MapGet("/", () => "Welcome to the Ocelot API Gateway!");

// Ocelot – должен быть в самом конце, и обязательно с await
await app.UseOcelot();

app.Run();