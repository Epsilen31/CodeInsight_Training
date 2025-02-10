using Codeinsight.VehicleInsights.Services.Constants;
using Codeinsight.VehicleInsights.Services.Contracts;
using Codeinsight.VehicleInsights.Services.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Codeinsight.VehicleInsights.Services.Features
{
    public class GenerateVehicleReport
    {
        public class Query : IRequest<Unit>
        {
            public string PathValue { get; }

            public Query(string pathValue)
            {
                PathValue = pathValue;
            }
        }

        public class Handler : IRequestHandler<Query, Unit>
        {
            private readonly ICarsDataHelper _reportHelper;
            private readonly IFileHandler _fileHandler;
            private readonly ILogger<GenerateVehicleReport> _logger;

            public Handler(
                IFileHandler fileHandler,
                ILogger<GenerateVehicleReport> logger,
                ICarsDataHelper reportHelper
            )
            {
                _fileHandler = fileHandler;
                _logger = logger;
                _reportHelper = reportHelper;
            }

            public async Task<Unit> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(request.PathValue))
                    {
                        _logger.LogError("File path cannot be empty.");
                        return Unit.Value;
                    }
                    var carsData = await _reportHelper.CarsReportCommonHelperAsyncAsync(
                        request.PathValue,
                        cancellationToken
                    );
                    await StoreCarsDataAsync(request.PathValue, carsData, cancellationToken);
                    return Unit.Value;
                }
                catch (Exception exception)
                {
                    _logger.LogError(
                        "Error generating vehicle report for file path: {FilePath}",
                        request.PathValue
                    );
                    throw new Exception(exception.Message);
                }
            }

            private async Task StoreCarsDataAsync(
                string filepath,
                ICollection<CarDto> cars,
                CancellationToken cancellationToken
            )
            {
                foreach (var car in cars)
                {
                    string fileName = Path.Combine(filepath, $"{car.Company}_{car.Model}.txt");
                    string carDetails = await FormatCarDetailsAsync(car);
                    await _fileHandler.GenerateFileAsync(fileName, carDetails, cancellationToken);
                }
            }

            private async Task<string> FormatCarDetailsAsync(CarDto car)
            {
                return await Task.Run(
                    () =>
                        $"{TableHeaderConstants.Model}: {car.Model}\n"
                        + $"{TableHeaderConstants.Company}: {car.Company}\n"
                        + $"{TableHeaderConstants.ManufacturingYear}: {car.ManufacturingYear}\n"
                        + $"{TableHeaderConstants.BasePrice}: {car.BasePrice}\n"
                        + $"{TableHeaderConstants.InsurancePrice}: {car.InsurancePrice}\n"
                        + $"{TableHeaderConstants.AfterTotalPrice}: {car.AfterTotalPrice}\n"
                        + $"{TableHeaderConstants.Rating}: {car.Rating}\n"
                );
            }
        }
    }
}
