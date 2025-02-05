using Codeinsight.Core.Settings;
using Codeinsight.WebApi.Extensions;

namespace Codeinsight.WebApi
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
            services.Configure<FilePaths>(configuration.GetSection("FilePaths"));
        }

        public static void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
