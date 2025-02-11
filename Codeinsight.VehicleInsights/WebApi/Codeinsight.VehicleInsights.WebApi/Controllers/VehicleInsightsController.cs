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

        public VehicleInsightsController(IMediator mediator, IOptions<FilePaths> filePathsOptions)
        {
            _mediator = mediator;
            _filePaths = filePathsOptions.Value;
        }

        [HttpGet(RouteKey.GenerateReport)]
        public async Task<IActionResult> GenerateVehicleReportAsync(
            CancellationToken cancellationToken
        )
        {
            await _mediator.Send(
                new GenerateVehicleReport.Query(_filePaths.BaseFile),
                cancellationToken
            );
            return Ok("Vehicle report generated successfully.");
        }

        [HttpGet(RouteKey.DisplayReport)]
        public async Task<IActionResult> DisplayVehicleReportInTabularAsync(
            CancellationToken cancellationToken
        )
        {
            await _mediator.Send(
                new DisplayVehicleReportInTabular.Query(_filePaths.BaseFile),
                cancellationToken
            );
            return Ok("Vehicle report displayed successfully.");
        }

        [HttpGet(RouteKey.SearchByModel)]
        public async Task<IActionResult> SearchVehicleByModelAsync(
            string model,
            CancellationToken cancellationToken
        )
        {
            var result = await _mediator.Send(
                new SearchVehicleByModel.Query(model, _filePaths.BaseFile),
                cancellationToken
            );
            return Ok(result);
        }

        [HttpGet(RouteKey.FilterByYear)]
        public async Task<IActionResult> GetFilterVehiclesByManufacturingYearAsync(
            int year,
            CancellationToken cancellationToken
        )
        {
            var result = await _mediator.Send(
                new GetFilterVehiclesByManufacturingYear.Query(year, _filePaths.BaseFile),
                cancellationToken
            );
            return Ok(result);
        }

        [HttpGet(RouteKey.SortByPrice)]
        public async Task<IActionResult> GetSortedVehiclesByPriceAsync(
            CancellationToken cancellationToken,
            int sortOrder = 1,
            int sortCriteria = 1
        )
        {
            var result = await _mediator.Send(
                new GetSortedVehiclesByPrice.Query(sortOrder, sortCriteria, _filePaths.BaseFile),
                cancellationToken
            );
            return Ok(result);
        }

        [HttpGet(RouteKey.AverageRating)]
        public async Task<IActionResult> GetVehiclesAverageRatingAsync(
            CancellationToken cancellationToken
        )
        {
            var result = await _mediator.Send(
                new GetVehiclesAverageRating.Query(_filePaths.BaseFile),
                cancellationToken
            );
            return Ok(result);
        }

        [HttpGet(RouteKey.CountByRating)]
        public async Task<IActionResult> GetVehiclesCountBasedOnRatingAsync(
            CancellationToken cancellationToken
        )
        {
            var result = await _mediator.Send(
                new GetVehiclesCountBasedOnRating.Query(_filePaths.BaseFile),
                cancellationToken
            );
            return Ok(result);
        }
    }
}
