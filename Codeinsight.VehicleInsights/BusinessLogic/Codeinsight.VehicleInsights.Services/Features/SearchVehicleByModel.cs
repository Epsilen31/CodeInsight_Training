using Codeinsight.VehicleInsights.Services.Contracts;
using Codeinsight.VehicleInsights.Services.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Codeinsight.VehicleInsights.Services.Features
{
    public class SearchVehicleByModel
    {
        public class Query : IRequest<ICollection<CarDto>>
        {
            public string Model { get; set; }
            public string FilePath { get; }

            public Query(string model, string filePath)
            {
                Model = model;
                FilePath = filePath;
            }
        }

        public class Handler : IRequestHandler<Query, ICollection<CarDto>>
        {
            private readonly ICarsDataHelper _reportHelper;
            private readonly ILogger<SearchVehicleByModel> _logger;

            public Handler(ILogger<SearchVehicleByModel> logger, ICarsDataHelper reportHelper)
            {
                _logger = logger;
                _reportHelper = reportHelper;
            }

            public async Task<ICollection<CarDto>> Handle(
                Query request,
                CancellationToken cancellationToken
            )
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(request.FilePath))
                    {
                        _logger.LogError("File path cannot be empty.");
                        return [];
                    }
                    if (string.IsNullOrWhiteSpace(request.Model))
                    {
                        _logger.LogError("Vehicle model cannot be empty.");
                        return [];
                    }

                    var carsData = await _reportHelper.CarsReportCommonHelperAsyncAsync(
                        request.FilePath,
                        cancellationToken
                    );
                    var filteredCars = carsData.Where(car =>
                        car.Model.Contains(request.Model, StringComparison.CurrentCultureIgnoreCase)
                    );
                    return [.. filteredCars];
                }
                catch (Exception exception)
                {
                    _logger.LogError(
                        "Error in SearchVehicleByModel: {Exception}",
                        exception.Message
                    );
                    throw new Exception(exception.Message);
                }
            }
        }
    }
}
