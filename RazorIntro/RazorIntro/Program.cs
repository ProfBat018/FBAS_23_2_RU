var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages(); // Добавляет поддержку Razor Pages

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting(); // Маршрутизация проходит через рефлексию, добавляем его в проект.


// Все что находится в папке wwwroot добавляется здесь. Там css и js 
app.MapStaticAssets(); 

app.MapRazorPages() // Используем ранее добавленный сервис
    .WithStaticAssets();



app.Run();