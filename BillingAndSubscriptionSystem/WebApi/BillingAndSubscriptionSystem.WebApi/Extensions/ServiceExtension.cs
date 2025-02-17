using BillingAndSubscriptionSystem.DataAccess;

namespace BillingAndSubscriptionSystem.WebApi.Extensions
{
    public static class ServiceExtension
    {
        public static void GetServices(this IServiceCollection services)
        {
            services.AddScoped<UnitOfWork>();
        }
    }
}
