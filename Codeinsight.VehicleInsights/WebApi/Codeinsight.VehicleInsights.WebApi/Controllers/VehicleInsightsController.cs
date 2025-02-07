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
    public class VehicleInsightsController : ControllerBase
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
                if (string.IsNullOrWhiteSpace(_filePaths.BaseFile))
                    return BadRequest("File path cannot be empty.");

                await _mediator.Send(
                    new GenerateVehicleReport.Query(_filePaths.BaseFile),
                    cancellationToken
                );
                return Ok("Vehicle report generated successfully.");
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    "Error generating vehicle report : {Exception}",
                    exception.Message
                );
                return StatusCode(500, $"Error generating vehicle report: {exception.Message}");
            }
        }

        [HttpGet(RouteKey.DisplayReport)]
        public async Task<IActionResult> DisplayVehicleReportInTabularAsync(
            CancellationToken cancellationToken
        )
        {
            try
            {
                Console.WriteLine($"getting file path: {_filePaths.BaseFile}");
                if (string.IsNullOrWhiteSpace(_filePaths.BaseFile))
                    return BadRequest("File path cannot be empty.");

                await _mediator.Send(
                    new DisplayVehicleReportInTabular.Query(_filePaths.BaseFile),
                    cancellationToken
                );
                return Ok("Vehicle report displayed successfully.");
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    "Error displaying vehicle report : {Exception}",
                    exception.Message
                );
                return StatusCode(500, $"Error displaying vehicle report: {exception.Message}");
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
                if (string.IsNullOrWhiteSpace(model))
                    return BadRequest("Vehicle model cannot be empty.");

                if (string.IsNullOrWhiteSpace(_filePaths.BaseFile))
                    return BadRequest("File path cannot be empty.");

                var result = await _mediator.Send(
                    new SearchVehicleByModel.Query(model, _filePaths.BaseFile),
                    cancellationToken
                );
                if (result == null || result.Count == 0)
                    return NotFound($"No vehicles found for the model: {model}.");

                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    "Error searching vehicle by model: {Exception}",
                    exception.Message
                );
                return StatusCode(
                    500,
                    $"Error searching vehicle by model '{model}': {exception.Message}"
                );
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
                if (year <= 0)
                    return BadRequest("Invalid manufacturing year specified.");

                if (string.IsNullOrWhiteSpace(_filePaths.BaseFile))
                    return BadRequest("File path cannot be empty.");

                var result = await _mediator.Send(
                    new GetFilterVehiclesByManufacturingYear.Query(year, _filePaths.BaseFile),
                    cancellationToken
                );
                if (result == null || result.Count == 0)
                    return NotFound($"No vehicles found for the manufacturing year: {year}.");

                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    "Error while filtering vehicles by manufacturing year: {Exception}",
                    exception.Message
                );
                return StatusCode(
                    500,
                    $"Error filtering vehicles by manufacturing year '{year}': {exception.Message}"
                );
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
                if (sortOrder < 1 || sortCriteria < 1)
                    return BadRequest("Invalid sort criteria. Use '1' for price or '2' for model.");

                if (string.IsNullOrWhiteSpace(_filePaths.BaseFile))
                    return BadRequest("File path cannot be empty.");

                var result = await _mediator.Send(
                    new GetSortedVehiclesByPrice.Query(
                        sortOrder,
                        sortCriteria,
                        _filePaths.BaseFile
                    ),
                    cancellationToken
                );
                if (result == null || !result.Any())
                    return NotFound("No vehicles available for sorting.");

                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    "Error while sorting vehicles by price: {Exception}",
                    exception.Message
                );
                return StatusCode(500, $"Error sorting vehicles by price: {exception.Message}");
            }
        }

        [HttpGet(RouteKey.AverageRating)]
        public async Task<IActionResult> GetVehiclesAverageRatingAsync(
            CancellationToken cancellationToken
        )
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_filePaths.BaseFile))
                    return BadRequest("File path cannot be empty.");

                var result = await _mediator.Send(
                    new GetVehiclesAverageRating.Query(_filePaths.BaseFile),
                    cancellationToken
                );
                if (result == null || result.Count == 0)
                    return NotFound("No rating data available for vehicles.");

                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    "Error while getting vehicles average rating: {Exception}",
                    exception.Message
                );
                return StatusCode(
                    500,
                    $"Error retrieving vehicle average rating: {exception.Message}"
                );
            }
        }

        [HttpGet(RouteKey.CountByRating)]
        public async Task<IActionResult> GetVehiclesCountBasedOnRatingAsync(
            CancellationToken cancellationToken
        )
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_filePaths.BaseFile))
                    return BadRequest("File path cannot be empty.");

                var result = await _mediator.Send(
                    new GetVehiclesCountBasedOnRating.Query(_filePaths.BaseFile),
                    cancellationToken
                );
                if (result == null || result.Count == 0)
                    return NotFound("No vehicle count data found for ratings.");

                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    "Error while getting vehicles count based on rating: {Exception}",
                    exception.Message
                );
                return StatusCode(
                    500,
                    $"Error retrieving vehicle count based on rating: {exception.Message}"
                );
            }
        }
    }
}
