using BillingAndSubscriptionSystem.Services.DTOs;
using BillingAndSubscriptionSystem.Services.Features;
using BillingAndSubscriptionSystem.Services.Features.UserSubscription;
using BillingAndSubscriptionSystem.WebApi.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BillingAndSubscriptionSystem.WebApi.Controllers
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

        [HttpPost(RouteKey.CreateUserSubscriptionPlan)]
        public async Task<IActionResult> CreateUserSubscriptionPlan(
            SubscriptionDto subscriptionDto,
            CancellationToken cancellationToken
        )
        {
            await _mediator.Send(
                new CreateUserSubscriptionPlan.Query(subscriptionDto),
                cancellationToken
            );
            return Ok(new { Message = "Subscription created successfully." });
        }

        [HttpPut(RouteKey.UpdateUserSubscriptionPlan)]
        public async Task<IActionResult> UpdateUserSubscriptionPlan(
            SubscriptionDto subscriptionDto,
            CancellationToken cancellationToken
        )
        {
            await _mediator.Send(
                new UpdateUserSubscriptionPlan.Query(subscriptionDto),
                cancellationToken
            );
            return Ok(new { Message = "Subscription updated successfully." });
        }

        [HttpGet(RouteKey.GetSubscriptionByUserId)]
        public async Task<IActionResult> GetSubscriptionByUserId(
            int userId,
            CancellationToken cancellationToken
        )
        {
            var subscription = await _mediator.Send(
                new GetSubscriptionByUserId.Query(new SubscriptionDto { UserId = userId }),
                cancellationToken
            );
            return Ok(
                new
                {
                    Message = "Subscription Data retrieved successfully.",
                    Subscription = subscription,
                }
            );
        }
    }
}
