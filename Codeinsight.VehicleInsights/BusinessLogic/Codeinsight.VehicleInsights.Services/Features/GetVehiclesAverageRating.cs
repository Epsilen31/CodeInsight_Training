using Codeinsight.VehicleInsights.Services.Contracts;
using Codeinsight.VehicleInsights.Services.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Codeinsight.VehicleInsights.Services.Features
{
    public class GetVehiclesAverageRating
    {
        public class Query : IRequest<ICollection<AverageRatingDto>>
        {
            public string FilePath { get; }

            public Query(string filePath)
            {
                FilePath = filePath;
            }
        }

        public class Handler : IRequestHandler<Query, ICollection<AverageRatingDto>>
        {
            private readonly ICarsDataHelper _reportHelper;
            private readonly ILogger<GetVehiclesAverageRating> _logger;

            public Handler(ILogger<GetVehiclesAverageRating> logger, ICarsDataHelper reportHelper)
            {
                _logger = logger;
                _reportHelper = reportHelper;
            }

            public async Task<ICollection<AverageRatingDto>> Handle(
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
                    ICollection<AverageRatingDto> averageRatings =
                    [
                        .. carsData
                            .GroupBy(car => car.Company)
                            .Select(group => new AverageRatingDto
                            {
                                Company = group.Key,
                                Rating = group
                                    .Average(car => Convert.ToDouble(car.Rating))
                                    .ToString("0.00"),
                            }),
                    ];
                    if (averageRatings == null || averageRatings.Count == 0)
                        return [];

                    return averageRatings;
                }
                catch (Exception exception)
                {
                    _logger.LogError(
                        "Error getting vehicles average rating {Exception}",
                        exception.Message
                    );
                    throw new Exception(exception.Message);
                }
            }
        }
    }
}
