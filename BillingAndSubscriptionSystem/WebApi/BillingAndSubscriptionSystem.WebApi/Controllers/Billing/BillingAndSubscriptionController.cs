using BillingAndSubscriptionSystem.Services.DTOs;
using BillingAndSubscriptionSystem.Services.Features.Billing;
using BillingAndSubscriptionSystem.Services.Features.Billings;
using BillingAndSubscriptionSystem.WebApi.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BillingAndSubscriptionSystem.WebApi.Controllers
{
    [ApiController]
    [Route(RouteKey.BillingRoute)]
    public class BillingController : BaseController
    {
        private readonly IMediator _mediator;

        public BillingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpPut(RouteKey.UpdateBilling)]
        public async Task<IActionResult> UpdateBilling(
            [FromBody] BillingDto billingDto,
            CancellationToken cancellationToken
        )
        {
            await _mediator.Send(new UpdateBillingDetails.Command(billingDto), cancellationToken);
            return Ok(new { Message = "Billing updated successfully." });
        }

        [Authorize]
        [HttpGet(RouteKey.GetUsersWithBilling)]
        public async Task<IActionResult> GetUsersWithBilling(
            [FromQuery] string userId,
            CancellationToken cancellationToken
        )
        {
            var usersWithBilling = await _mediator.Send(
                new GetAllUsersWithBillingDetails.Query(int.Parse(userId)),
                cancellationToken
            );

            return Ok(
                new
                {
                    UsersWithBilling = usersWithBilling,
                    Message = "Users with billing data fetched successfully.",
                }
            );
        }
    }
}
