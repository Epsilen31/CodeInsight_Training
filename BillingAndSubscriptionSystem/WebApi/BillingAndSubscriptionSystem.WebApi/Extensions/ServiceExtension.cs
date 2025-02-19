using BillingAndSubscriptionSystem.Core.Cache;
using BillingAndSubscriptionSystem.Core.Contracts;
using BillingAndSubscriptionSystem.DataAccess;
using BillingAndSubscriptionSystem.Services.Contracts;
using BillingAndSubscriptionSystem.Services.Services;

namespace BillingAndSubscriptionSystem.WebApi.Extensions
{
    public static class ServiceExtension
    {
        public static void GetServices(this IServiceCollection services)
        {
            services.AddScoped<UnitOfWork>();
            services.AddSingleton<IRedisFactory, RedisFactory>();
            services.AddSingleton<IRedisService, RedisService>();
            services.AddSingleton<ITokenService, TokenService>();
        }
    }
}
