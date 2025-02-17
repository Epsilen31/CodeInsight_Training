using BillingAndSubscriptionSystem.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

app.UseRouting();
app.MapControllers();

app.Run();
