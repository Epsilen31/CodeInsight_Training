using BillingAndSubscriptionSystem.Services.DTOs;
using BillingAndSubscriptionSystem.Services.Features.Authentication;
using BillingAndSubscriptionSystem.WebApi.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BillingAndSubscriptionSystem.WebApi.Controllers.Auth
{
    [ApiController]
    [Route(RouteKey.AuthRoute)]
    public class LoginController : BaseController
    {
        private readonly IMediator _mediator;

        public LoginController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(RouteKey.Login)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var loginResult = await _mediator.Send(new LoginUser.Command(loginDto));
            if (loginResult == null || string.IsNullOrEmpty(loginResult.Token))
            {
                return Unauthorized("Invalid credentials");
            }

            return Ok(
                new
                {
                    Token = loginResult.Token,
                    Name = loginResult.Name,
                    Email = loginResult.Email,
                    Role = loginResult.Role,
                }
            );
        }
    }
}
