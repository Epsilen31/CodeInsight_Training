using Codeinsight.VehicleInsights.Services.Contracts;
using Codeinsight.VehicleInsights.Services.DTOs;
using Codeinsight.VehicleInsights.Services.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Codeinsight.VehicleInsights.Services.Features
{
    public class GetSortedVehiclesByPrice
    {
        public class Query : IRequest<ICollection<CarDto>>
        {
            public int SortOrder { get; set; }
            public int SortCriteria { get; set; }
            public string FilePath { get; }

            public Query(int sortOrder, int sortCriteria, string filePath)
            {
                SortOrder = sortOrder;
                SortCriteria = sortCriteria;
                FilePath = filePath;
            }
        }

        public class Handler : IRequestHandler<Query, ICollection<CarDto>>
        {
            private readonly ICarsDataHelper _reportHelper;
            private readonly ILogger<GetSortedVehiclesByPrice> _logger;

            public Handler(ILogger<GetSortedVehiclesByPrice> logger, ICarsDataHelper reportHelper)
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
                    if (request.SortOrder < 1 || request.SortCriteria < 1)
                    {
                        _logger.LogError(
                            "Invalid sort criteria. Use '1' for price or '2' for model."
                        );

                        return new List<CarDto>();
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
                    return await CarByPriceAsync(
                        (SortCriteria)request.SortCriteria,
                        (SortOrder)request.SortOrder,
                        carsData
                    );
                }
                catch (Exception exception)
                {
                    _logger.LogError(
                        "Error getting sorted vehicles by price: {Exception}",
                        exception.Message
                    );
                    throw new Exception(exception.Message);
                }
            }

            private async Task<ICollection<CarDto>> CarByPriceAsync(
                SortCriteria sortCriteria,
                SortOrder sortOrder,
                ICollection<CarDto> carDetails
            )
            {
                switch (sortCriteria)
                {
                    case SortCriteria.BasePrice:
                        carDetails =
                            sortOrder == SortOrder.Ascending
                                ? [.. carDetails.OrderBy(car => car.BasePrice)]
                                : carDetails.OrderByDescending(car => car.BasePrice).ToList();
                        return await Task.Run(() => carDetails);

                    case SortCriteria.AfterTotalPrice:
                        carDetails =
                            sortOrder == SortOrder.Ascending
                                ? [.. carDetails.OrderBy(car => car.AfterTotalPrice)]
                                : [.. carDetails.OrderByDescending(car => car.AfterTotalPrice)];
                        return await Task.Run(() => carDetails);

                    default:
                        _logger.LogWarning("Invalid Sort Choice. Please choose either 1 or 2.");
                        return await Task.Run(() => new List<CarDto>());
                }
            }
        }
    }
}
