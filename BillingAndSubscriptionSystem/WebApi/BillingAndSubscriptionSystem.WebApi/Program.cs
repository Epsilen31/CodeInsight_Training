using System.Text.Json.Serialization;
using BillingAndSubscriptionSystem.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices(builder.Configuration);

builder
    .Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

var app = builder.Build();

app.UseRouting();
app.UseCors("AllowSpecificOrigin");
app.UseWebSockets();
app.UseAuthentication();
app.UseAuthorization();

// app.MapHub<NotificationHub>("/notificationHub").RequireAuthorization();
app.MapControllers();

app.Run();
