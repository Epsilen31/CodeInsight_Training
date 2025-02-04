using Codeinsight.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

Startup.Configure(app);

app.Run();
