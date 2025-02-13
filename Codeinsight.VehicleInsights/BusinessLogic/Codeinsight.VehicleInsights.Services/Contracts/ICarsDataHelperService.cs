using Codeinsight.VehicleInsights.Services.DTOs;

namespace Codeinsight.VehicleInsights.Services.Contracts
{
    public interface ICarsDataHelperServiceService
    {
        Task<ICollection<CarDto>> CarsReportCommonHelperAsyncAsync(
            string filePath,
            CancellationToken cancellationToken
        );
    }
}
