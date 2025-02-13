using BillingAndSubscriptionSystem.Services.DTOs;
using BillingAndSubscriptionSystem.Services.Features.Payments;
using BillingAndSubscriptionSystem.WebApi.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BillingAndSubscriptionSystem.WebApi.Controllers.Payments
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

        [HttpPost(RouteKey.CreatePayment)]
        public async Task<IActionResult> CreatePayment(PaymentDto payment)
        {
            await _mediator.Send(new PaymentProcess.Query(payment));
            return Ok(new { Message = "Payment created successfully." });
        }

        [HttpGet(RouteKey.GetOverDuePayment)]
        public async Task<IActionResult> GetOverduePayments(PaymentDto payment)
        {
            var overduePayments = await _mediator.Send(new GetOverduePayments.Query(payment));
            return Ok(overduePayments);
        }
    }
}
