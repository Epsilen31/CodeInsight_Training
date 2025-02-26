using BillingAndSubscriptionSystem.Core.Hubs;
using BillingAndSubscriptionSystem.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapHub<NotificationHub>("/notificationHub").RequireAuthorization();
app.MapControllers();

app.Run();
