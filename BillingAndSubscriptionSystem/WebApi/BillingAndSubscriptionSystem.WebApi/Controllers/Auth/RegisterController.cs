using BillingAndSubscriptionSystem.Services.DTOs;
using BillingAndSubscriptionSystem.Services.Features.Authentication;
using BillingAndSubscriptionSystem.WebApi.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BillingAndSubscriptionSystem.WebApi.Controllers.Auth
{
    [ApiController]
    [Route(RouteKey.AuthRoute)]
    public class RegisterController : BaseController
    {
        private readonly IMediator _mediator;

        public RegisterController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(RouteKey.Register)]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            await _mediator.Send(new RegisterUser.Query(userDto));
            return Ok(new { message = "User Signup Successful" });
        }
    }
}
