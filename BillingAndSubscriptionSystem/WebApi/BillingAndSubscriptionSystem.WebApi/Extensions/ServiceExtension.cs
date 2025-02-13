using BillingAndSubscriptionSystem.DataAccess;
using BillingAndSubscriptionSystem.DataAccess.Contracts;

namespace BillingAndSubscriptionSystem.WebApi.Extensions
{
    public static class ServiceExtension
    {
        public static void GetServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
