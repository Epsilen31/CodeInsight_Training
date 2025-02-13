using Codeinsight.VehicleInsights.Services.Contracts;
using Codeinsight.VehicleInsights.Services.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Codeinsight.VehicleInsights.Services.Features
{
    public class GetFilterVehiclesByManufacturingYear
    {
        public class Query : IRequest<ICollection<CarDto>>
        {
            public int ManufacturingYear { get; set; }
            public string FilePath { get; }

            public Query(int manufacturingYear, string filePath)
            {
                ManufacturingYear = manufacturingYear;
                FilePath = filePath;
            }
        }

        public class Handler : IRequestHandler<Query, ICollection<CarDto>>
        {
            private readonly ICarsDataHelperServiceService _reportHelper;
            private readonly ILogger<GetFilterVehiclesByManufacturingYear> _logger;

            public Handler(
                ILogger<GetFilterVehiclesByManufacturingYear> logger,
                ICarsDataHelperServiceService reportHelper
            )
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
                    if (request.ManufacturingYear <= 0)
                    {
                        _logger.LogError("Invalid manufacturing year specified.");
                        return [];
                    }
                    if (string.IsNullOrWhiteSpace(request.FilePath))
                    {
                        _logger.LogError("File path cannot be empty.");
                        return [];
                    }
                    var carsData = await _reportHelper.CarsReportCommonHelperAsyncAsync(
                        request.FilePath,
                        cancellationToken
                    );

                    var filteredCars = carsData.Where(car =>
                        int.TryParse(car.ManufacturingYear, out int year)
                        && year == request.ManufacturingYear
                    );

                    if (filteredCars == null || !filteredCars.Any())
                    {
                        _logger.LogWarning(
                            "No cars found for the specified manufacturing year: {ManufacturingYear}",
                            request.ManufacturingYear
                        );
                        return [];
                    }
                    return [.. filteredCars];
                }
                catch (Exception exception)
                {
                    _logger.LogError(
                        exception,
                        "Error while getting cars data : {Exception}",
                        exception.Message
                    );
                    throw new InvalidOperationException(exception.Message);
                }
            }
        }
    }
}
