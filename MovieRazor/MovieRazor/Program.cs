var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.MapRazorPages()
    .WithStaticAssets();

app.MapFallbackToPage("/Main"); 

app.MapStaticAssets();

app.MapStaticAssets();
app.Run();