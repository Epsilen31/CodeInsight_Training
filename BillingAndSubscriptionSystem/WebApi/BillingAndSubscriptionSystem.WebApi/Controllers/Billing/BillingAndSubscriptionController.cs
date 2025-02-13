using BillingAndSubscriptionSystem.Services.DTOs;
using BillingAndSubscriptionSystem.Services.Features.Billing;
using BillingAndSubscriptionSystem.Services.Features.Billings;
using BillingAndSubscriptionSystem.WebApi.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BillingAndSubscriptionSystem.WebApi.Controllers.Billing
{
    [ApiController]
    [Route(RouteKey.MainRoute)]
    public class BillingAndSubscriptionController : BaseController
    {
        private readonly IMediator _mediator;

        public BillingAndSubscriptionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut(RouteKey.UpdateBilling)]
        public async Task<IActionResult> UpdateBilling(
            BillingDto billingDto,
            CancellationToken cancellationToken
        )
        {
            await _mediator.Send(new UpdateBillingDetails.Query(billingDto), cancellationToken);
            return Ok(new { Message = "Billing updated successfully." });
        }

        [HttpGet(RouteKey.GetUsersWithBilling)]
        public async Task<IActionResult> GetUsersWithBilling(
            BillingDto billing,
            CancellationToken cancellationToken
        )
        {
            var usersWithBilling = await _mediator.Send(
                new GetAllUsersWithBillingDetails.Query(billing),
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
