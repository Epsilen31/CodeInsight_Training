using Codeinsight.VehicleInsights.Core.Settings;
using Codeinsight.VehicleInsights.Services.Contracts;
using Codeinsight.VehicleInsights.Services.Services;

namespace Codeinsight.VehicleInsights.WebApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void GetServices(this IServiceCollection services)
        {
            services.AddScoped<IFileHandler, FileHandler>();
            services.AddScoped<ICarsDataHelper, CarsDataHelper>();
            services.AddScoped<FilePaths>();
        }
    }
}
