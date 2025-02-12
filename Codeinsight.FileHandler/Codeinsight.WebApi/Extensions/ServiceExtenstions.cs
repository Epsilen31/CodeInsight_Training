using Codeinsight.Services.Contracts;
using Codeinsight.Services.Services;

namespace Codeinsight.WebApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void GetServices(this IServiceCollection services)
        {
            services.AddScoped<IFileHandler, FileHandler>();
        }
    }
}
