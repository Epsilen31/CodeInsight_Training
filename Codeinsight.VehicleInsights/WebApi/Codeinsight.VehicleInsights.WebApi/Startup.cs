using Codeinsight.VehicleInsights.Core.Settings;
using Codeinsight.VehicleInsights.Services.Services;
using Codeinsight.VehicleInsights.WebApi.Extensions;
using Microsoft.Extensions.Options;

namespace Codeinsight.VehicleInsights.WebApi
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
            services.AddMediatR(report =>
                report.RegisterServicesFromAssembly(typeof(CarsDataHelper).Assembly)
            );

            services.Configure<FilePaths>(configuration.GetSection("FilePaths"));

            services.AddSingleton(file => file.GetRequiredService<IOptions<FilePaths>>().Value);
        }
    }
}
