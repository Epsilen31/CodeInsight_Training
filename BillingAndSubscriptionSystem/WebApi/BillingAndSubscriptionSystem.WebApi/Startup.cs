using System.Reflection;
using System.Text;
using BillingAndSubscriptionSystem.Context;
using BillingAndSubscriptionSystem.Core.Exceptions;
using BillingAndSubscriptionSystem.Core.Settings;
using BillingAndSubscriptionSystem.Services.Contracts;
using BillingAndSubscriptionSystem.WebApi.Authorization.Policy;
using BillingAndSubscriptionSystem.WebApi.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BillingAndSubscriptionSystem.WebApi
{
    public static class Startup
    {
        public static void ConfigureServices(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddControllers();
            services.GetServices();

            string? secretKey = configuration["Jwt:Secret"];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new CustomException("JWT Secret is missing in configuration.", null);
            }

            services.Configure<AppSetting>(configuration.GetSection("AppSetting"));
            services.AddSingleton(resolver =>
                resolver.GetRequiredService<IOptions<AppSetting>>().Value
            );

            var connectionString =
                configuration.GetConnectionString("DefaultConnection")
                ?? throw new CustomException("Database connection string is missing.", null);

            services.AddDbContext<BillingDbContext>(options =>
                options.UseMySql(
                    connectionString,
                    new MySqlServerVersion(new Version(8, 0, 23)),
                    b => b.MigrationsAssembly("BillingAndSubscriptionSystem.WebApi")
                )
            );

            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssemblies(
                    Assembly.GetExecutingAssembly(),
                    typeof(IMediatorService).Assembly
                )
            );

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration =
                    configuration.GetConnectionString("Redis")
                    ?? throw new CustomException("Redis connection string is missing.", null);
            });

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(secretKey)
                        ),
                    }
                );

            services.AddAuthorization(options =>
            {
                RolePolicyRules.RegisterPolicies(options);
            });
        }
    }
}
