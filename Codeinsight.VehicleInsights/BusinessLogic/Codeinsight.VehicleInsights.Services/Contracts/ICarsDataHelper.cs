using Codeinsight.VehicleInsights.Services.DTOs;

namespace Codeinsight.VehicleInsights.Services.Contracts
{
    public interface ICarsDataHelper
    {
        Task<ICollection<CarDto>> CarsReportCommonHelperAsyncAsync(
            string filePath,
            CancellationToken token
        );
    }
}
