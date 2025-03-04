using System.Security.Claims;
using BillingAndSubscriptionSystem.Services.Features.Menu;
using BillingAndSubscriptionSystem.WebApi.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BillingAndSubscriptionSystem.WebApi.Controllers.Menu
{
    [ApiController]
    [Route(RouteKey.MenuRoute)]
    public class MenuController : BaseController
    {
        private readonly IMediator _mediator;

        public MenuController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpGet(RouteKey.GetSidebarMenu)]
        public async Task<IActionResult> GetSidebarMenu()
        {
            var userRole =
                User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "Admin";

            Console.WriteLine($"userRole = {userRole}");
            var menu = await _mediator.Send(new GetSidebarMenu.Query(userRole));

            return Ok(new { Message = "Menu retrieved successfully.", Menu = menu });
        }
    }
}
