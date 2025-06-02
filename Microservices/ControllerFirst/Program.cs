using ControllerFirst.DTO.Requests;
using ControllerFirst.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(opts =>
{
    opts.ListenAnyIP(5001);
});

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var app = builder.Build();

startup.Configure(app);


// using (var scope = app.Services.CreateScope())
// {
//     var reindexService = scope.ServiceProvider.GetRequiredService<IElasticReindexService<UserElasticDTO>>();
//     await reindexService.ReindexAllAsync();
// }

app.Run();
