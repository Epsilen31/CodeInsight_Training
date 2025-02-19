using BillingAndSubscriptionSystem.Services.DTOs;
using BillingAndSubscriptionSystem.Services.Features;
using BillingAndSubscriptionSystem.Services.Features.UserSubscription;
using BillingAndSubscriptionSystem.WebApi.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BillingAndSubscriptionSystem.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route(RouteKey.UserSubscriptionRoute)]
    public class UserSubscriptionController : BaseController
    {
        private readonly IMediator _mediator;

        public UserSubscriptionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(RouteKey.CreateUserSubscriptionPlan)]
        public async Task<IActionResult> CreateUserSubscriptionPlan(
            [FromBody] SubscriptionDto subscriptionDto,
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
            int Id,
            [FromBody] SubscriptionDto subscriptionDto,
            CancellationToken cancellationToken
        )
        {
            subscriptionDto.SubscriptionId = Id;
            await _mediator.Send(
                new UpdateUserSubscriptionPlan.Query(subscriptionDto),
                cancellationToken
            );
            return Ok(new { Message = "Subscription updated successfully." });
        }

        [HttpGet(RouteKey.GetSubscriptionByUserId)]
        public async Task<IActionResult> GetSubscriptionByUserId(
            [FromRoute] int userId,
            CancellationToken cancellationToken
        )
        {
            var subscription = await _mediator.Send(
                new GetSubscriptionByUserId.Query(userId),
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
