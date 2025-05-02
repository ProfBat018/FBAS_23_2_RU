using Azure.Identity;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

var keyVaultName = "FuncVault232";
var kvUri = $"https://{keyVaultName}.vault.azure.net/";

var credential = new ChainedTokenCredential(
    new AzureCliCredential(),
    new VisualStudioCredential() 
);

builder.Configuration.AddAzureKeyVault(new Uri(kvUri), new DefaultAzureCredential());


builder.ConfigureFunctionsWebApplication();

var app = builder.Build();


app.Run();

