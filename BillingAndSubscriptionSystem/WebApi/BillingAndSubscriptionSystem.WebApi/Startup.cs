using BillingAndSubscriptionSystem.Context;
using BillingAndSubscriptionSystem.Core.Settings;
using BillingAndSubscriptionSystem.WebApi.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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

            //  Bind AppSettings (if other settings exist)
            services.Configure<AppSetting>(configuration.GetSection("AppSetting"));
            services.AddSingleton(resolver =>
                resolver.GetRequiredService<IOptions<AppSetting>>().Value
            );

            // Get Connection String Properly
            var connectionString =
                configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Database connection string is missing.");

            //  Register DbContext with MySQL Configuration
            services.AddDbContext<BillingDbContext>(options =>
                options.UseMySql(
                    connectionString,
                    new MySqlServerVersion(new Version(8, 0, 21)),
                    b => b.MigrationsAssembly("BillingAndSubscriptionSystem.WebApi")
                )
            );
        }
    }
}
