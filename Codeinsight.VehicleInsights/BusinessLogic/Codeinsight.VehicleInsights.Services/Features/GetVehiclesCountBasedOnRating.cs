using Codeinsight.VehicleInsights.Services.Contracts;
using Codeinsight.VehicleInsights.Services.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Codeinsight.VehicleInsights.Services.Features
{
    public class GetVehiclesCountBasedOnRating
    {
        public class Query : IRequest<ICollection<CarDto>>
        {
            public string FilePath { get; }

            public Query(string filePath)
            {
                FilePath = filePath;
            }
        }

        public class Handler : IRequestHandler<Query, ICollection<CarDto>>
        {
            private readonly ICarsDataHelperServiceService _reportHelper;
            private readonly ILogger<GetVehiclesCountBasedOnRating> _logger;

            public Handler(
                ILogger<GetVehiclesCountBasedOnRating> logger,
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
                    if (string.IsNullOrWhiteSpace(request.FilePath))
                    {
                        _logger.LogError("File path cannot be empty.");
                        return [];
                    }
                    var carsData = await _reportHelper.CarsReportCommonHelperAsyncAsync(
                        request.FilePath,
                        cancellationToken
                    );
                    var ratingGroup = carsData
                        .GroupBy(car => car.Rating)
                        .SelectMany(group => group)
                        .ToList();

                    return ratingGroup;
                }
                catch (Exception exception)
                {
                    _logger.LogError(
                        exception,
                        "Error occurred while getting vehicles count based on rating:{Exception}",
                        exception.Message
                    );
                    throw new InvalidOperationException(exception.Message);
                }
            }
        }
    }
}
