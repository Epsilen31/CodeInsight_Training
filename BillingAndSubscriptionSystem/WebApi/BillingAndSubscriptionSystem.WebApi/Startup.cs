using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using BillingAndSubscriptionSystem.Context;
using BillingAndSubscriptionSystem.Core.Exceptions;
using BillingAndSubscriptionSystem.Core.Settings;
using BillingAndSubscriptionSystem.Services.Contracts;
using BillingAndSubscriptionSystem.Services.Services;
using BillingAndSubscriptionSystem.WebApi.Authentication;
using BillingAndSubscriptionSystem.WebApi.Authorization.Policy;
using BillingAndSubscriptionSystem.WebApi.Extensions;
using Mapster;
using Microsoft.AspNetCore.Authentication;
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

            services.AddCors(options =>
            {
                options.AddPolicy(
                    "AllowSpecificOrigin",
                    builder =>
                        builder
                            .WithOrigins("http://localhost:4200")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials()
                );
            });

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration =
                    configuration.GetConnectionString("Redis")
                    ?? throw new CustomException("Redis connection string is missing.", null);
            });

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
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
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["token"];
                            var path = context.HttpContext.Request.Path;

                            if (
                                !string.IsNullOrEmpty(accessToken)
                                && path.StartsWithSegments("/notificationHub")
                            )
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        },
                    };
                });

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "JwtAndRedisAuthenticationScheme";
                    options.DefaultChallengeScheme = "JwtAndRedisAuthenticationScheme";
                })
                .AddScheme<AuthenticationSchemeOptions, CustomAuthenticationHandler>(
                    "JwtAndRedisAuthenticationScheme",
                    options => { }
                );

            services.AddAuthorization(options =>
            {
                RolePolicyRules.RegisterPolicies(options);
            });

            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(typeof(MapsterService).Assembly);

            services.AddSignalR(options =>
            {
                options.KeepAliveInterval = TimeSpan.FromSeconds(60);
                options.HandshakeTimeout = TimeSpan.FromSeconds(60);
                options.EnableDetailedErrors = true;
            });
        }
    }
}
