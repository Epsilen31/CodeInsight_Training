using Codeinsight.VehicleInsights.Core.Settings;
using Codeinsight.VehicleInsights.Services.Features;
using Codeinsight.VehicleInsights.WebApi.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Codeinsight.VehicleInsights.WebApi.Controllers
{
    [ApiController]
    [Route(RouteKey.HeadRoute)]
    public class VehicleInsightsController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly FilePaths _filePaths;
        private readonly ILogger<VehicleInsightsController> _logger;

        public VehicleInsightsController(
            IMediator mediator,
            IOptions<FilePaths> filePathsOptions,
            ILogger<VehicleInsightsController> logger
        )
        {
            _mediator = mediator;
            _filePaths = filePathsOptions.Value;
            _logger = logger;
        }

        [HttpGet(RouteKey.GenerateReport)]
        public async Task<IActionResult> GenerateVehicleReportAsync(
            CancellationToken cancellationToken
        )
        {
            try
            {
                await _mediator.Send(
                    new GenerateVehicleReport.Query(_filePaths.BaseFile),
                    cancellationToken
                );
                return Ok("Vehicle report generated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating vehicle report");
                return StatusCode(500, "Error generating vehicle report");
            }
        }

        [HttpGet(RouteKey.DisplayReport)]
        public async Task<IActionResult> DisplayVehicleReportInTabularAsync(
            CancellationToken cancellationToken
        )
        {
            try
            {
                await _mediator.Send(
                    new DisplayVehicleReportInTabular.Query(_filePaths.BaseFile),
                    cancellationToken
                );
                return Ok("Vehicle report displayed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error displaying vehicle report");
                return StatusCode(500, "Error displaying vehicle report");
            }
        }

        [HttpGet(RouteKey.SearchByModel)]
        public async Task<IActionResult> SearchVehicleByModelAsync(
            [FromRoute] string model,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var result = await _mediator.Send(
                    new SearchVehicleByModel.Query(model, _filePaths.BaseFile),
                    cancellationToken
                );
                if (result == null || result.Count == 0)
                    return NotFound($"No vehicles found for the model: {model}.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error searching vehicle by model '{model}'");
                return StatusCode(500, $"Error searching vehicle by model '{model}'");
            }
        }

        [HttpGet(RouteKey.FilterByYear)]
        public async Task<IActionResult> GetFilterVehiclesByManufacturingYearAsync(
            [FromQuery] int year,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var result = await _mediator.Send(
                    new GetFilterVehiclesByManufacturingYear.Query(year, _filePaths.BaseFile),
                    cancellationToken
                );
                if (result == null || result.Count == 0)
                    return NotFound($"No vehicles found for the manufacturing year: {year}.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error filtering vehicles by manufacturing year '{year}'");
                return StatusCode(500, $"Error filtering vehicles by manufacturing year '{year}'");
            }
        }

        [HttpGet(RouteKey.SortByPrice)]
        public async Task<IActionResult> GetSortedVehiclesByPriceAsync(
            [FromQuery] int sortOrder = 1,
            [FromQuery] int sortCriteria = 1,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                var result = await _mediator.Send(
                    new GetSortedVehiclesByPrice.Query(
                        sortOrder,
                        sortCriteria,
                        _filePaths.BaseFile
                    ),
                    cancellationToken
                );
                if (result == null || result.Count == 0)
                    return NotFound("No vehicles available for sorting.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sorting vehicles by price");
                return StatusCode(500, "Error sorting vehicles by price");
            }
        }

        [HttpGet(RouteKey.AverageRating)]
        public async Task<IActionResult> GetVehiclesAverageRatingAsync(
            CancellationToken cancellationToken
        )
        {
            try
            {
                var result = await _mediator.Send(
                    new GetVehiclesAverageRating.Query(_filePaths.BaseFile),
                    cancellationToken
                );
                if (result == null || result.Count == 0)
                    return NotFound("No rating data available for vehicles.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vehicle average rating");
                return StatusCode(500, "Error retrieving vehicle average rating");
            }
        }

        [HttpGet(RouteKey.CountByRating)]
        public async Task<IActionResult> GetVehiclesCountBasedOnRatingAsync(
            CancellationToken cancellationToken
        )
        {
            try
            {
                var result = await _mediator.Send(
                    new GetVehiclesCountBasedOnRating.Query(_filePaths.BaseFile),
                    cancellationToken
                );
                if (result == null || result.Count == 0)
                    return NotFound("No vehicle count data found for ratings.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vehicle count based on rating");
                return StatusCode(500, "Error retrieving vehicle count based on rating");
            }
        }
    }
}
