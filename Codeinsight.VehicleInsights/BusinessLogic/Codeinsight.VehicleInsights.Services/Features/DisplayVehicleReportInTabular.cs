using Codeinsight.VehicleInsights.Services.Constants;
using Codeinsight.VehicleInsights.Services.Contracts;
using Codeinsight.VehicleInsights.Services.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Codeinsight.VehicleInsights.Services.Features
{
    public class DisplayVehicleReportInTabular
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

            private readonly ILogger<DisplayVehicleReportInTabular> _logger;

            public Handler(
                ILogger<DisplayVehicleReportInTabular> logger,
                ICarsDataHelper reportHelper
            )
            {
                _logger = logger;
                _reportHelper = reportHelper;
            }

            public async Task<Unit> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    var carsData = await _reportHelper.CarsReportCommonHelperAsyncAsync(
                        request.PathValue,
                        cancellationToken
                    );
                    await DisplayAllCarsAsync(carsData, cancellationToken);
                    return Unit.Value;
                }
                catch (Exception exception)
                {
                    _logger.LogError(
                        "Error displaying vehicle report in tabular format: {ErrorMessage}",
                        exception.Message
                    );
                    throw new Exception(exception.Message);
                }
            }

            private Task DisplayAllCarsAsync(
                ICollection<CarDto> carDetails,
                CancellationToken cancellationToken
            )
            {
                _logger.LogInformation(
                    "{Model}\t{Company}\t{ManufacturingYear}\t{BasePrice}\t{InsurancePrice}\t{AfterTotalPrice}\t{Rating}",
                    TableHeaderConstants.Model,
                    TableHeaderConstants.Company,
                    TableHeaderConstants.ManufacturingYear,
                    TableHeaderConstants.BasePrice,
                    TableHeaderConstants.InsurancePrice,
                    TableHeaderConstants.AfterTotalPrice,
                    TableHeaderConstants.Rating
                );

                foreach (var car in carDetails)
                {
                    _logger.LogInformation(
                        "{Model}\t{Company}\t{ManufacturingYear}\t{BasePrice}\t{InsurancePrice}\t{AfterTotalPrice}\t{Rating}",
                        car.Model,
                        car.Company,
                        car.ManufacturingYear,
                        car.BasePrice,
                        car.InsurancePrice,
                        car.AfterTotalPrice,
                        car.Rating
                    );
                }
                return Task.CompletedTask;
            }
        }
    }
}
