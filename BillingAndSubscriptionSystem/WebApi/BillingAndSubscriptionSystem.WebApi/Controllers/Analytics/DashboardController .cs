using BillingAndSubscriptionSystem.Services.Features.Analytics;
using BillingAndSubscriptionSystem.WebApi.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BillingAndSubscriptionSystem.WebApi.Controllers.Analytics
{
    [ApiController]
    [Route(RouteKey.AnalyticsRoute)]
    public class DashboardController : BaseController
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpGet(RouteKey.GetAnalyticsState)]
        public async Task<IActionResult> GetDashboardStats()
        {
            var dashboardStats = await _mediator.Send(new GetDashboardStatsQuery.Query());
            return Ok(dashboardStats);
        }
    }
}
