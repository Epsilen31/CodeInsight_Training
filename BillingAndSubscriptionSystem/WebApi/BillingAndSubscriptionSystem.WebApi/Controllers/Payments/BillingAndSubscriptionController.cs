using BillingAndSubscriptionSystem.Services.DTOs;
using BillingAndSubscriptionSystem.Services.Features.Payments;
using BillingAndSubscriptionSystem.WebApi.Authorization.AdminAttribute;
using BillingAndSubscriptionSystem.WebApi.Authorization.Policy;
using BillingAndSubscriptionSystem.WebApi.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BillingAndSubscriptionSystem.WebApi.Controllers
{
    [ApiController]
    [Route(RouteKey.PaymentRoute)]
    public class PaymentController : BaseController
    {
        private readonly IMediator _mediator;

        public PaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost(RouteKey.CreatePayment)]
        public async Task<IActionResult> CreatePayment(
            [FromBody] PaymentDto payment,
            CancellationToken cancellationToken
        )
        {
            await _mediator.Send(new PaymentProcess.Command(payment), cancellationToken);

            return Ok(new { Message = "Payment created successfully." });
        }

        [RoleAuthorization(RolePolicyRules.ADMIN_ROLE)]
        [HttpGet(RouteKey.GetOverDuePayment)]
        public async Task<IActionResult> GetOverduePayments(
            [FromQuery] string subscriptionId,
            CancellationToken cancellationToken
        )
        {
            var overduePayments = await _mediator.Send(
                new GetOverduePayments.Query(int.Parse(subscriptionId)),
                cancellationToken
            );

            return Ok(
                new
                {
                    OverduePayments = overduePayments,
                    Message = "Overdue payments fetched successfully.",
                }
            );
        }
    }
}
