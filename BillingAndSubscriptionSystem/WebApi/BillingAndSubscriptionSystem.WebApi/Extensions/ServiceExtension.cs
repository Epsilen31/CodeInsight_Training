using BillingAndSubscriptionSystem.Core.BackGround;
using BillingAndSubscriptionSystem.Core.Cache;
using BillingAndSubscriptionSystem.Core.Contracts;
using BillingAndSubscriptionSystem.DataAccess;
using BillingAndSubscriptionSystem.DataAccess.Contracts;
using BillingAndSubscriptionSystem.Services.Contracts;
using BillingAndSubscriptionSystem.Services.Notification;
using BillingAndSubscriptionSystem.Services.Services;
using BillingAndSubscriptionSystem.WebApi.Authorization;
using BillingAndSubscriptionSystem.WebApi.Authorization.AdminAttribute.Handler;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;

namespace BillingAndSubscriptionSystem.WebApi.Extensions
{
    public static class ServiceExtension
    {
        public static void GetServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IRedisFactory, RedisFactory>();
            services.AddSingleton<IRedisService, RedisService>();
            services.AddSingleton<ITokenService, TokenService>();
            services.AddSingleton<IAuthorizationPolicyProvider, CustomPolicyProvider>();
            services.AddSingleton<IAuthorizationHandler, AdminHandler>();
            services.AddSingleton<BackGroundQueue>();
            services.AddHostedService<BackGroundService>();
            services.AddSingleton<IMapper, Mapper>();
            services.AddScoped<INotificationService, NotificationService>();
        }
    }
}
