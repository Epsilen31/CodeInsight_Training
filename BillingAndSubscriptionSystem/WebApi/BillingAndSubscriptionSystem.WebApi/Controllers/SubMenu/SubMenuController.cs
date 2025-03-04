using BillingAndSubscriptionSystem.Services.Features.SubMenu;
using BillingAndSubscriptionSystem.WebApi.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BillingAndSubscriptionSystem.WebApi.Controllers.SubMenu
{
    [ApiController]
    [Route(RouteKey.SubMenuRoute)]
    public class SubMenuController : BaseController
    {
        private readonly IMediator _mediator;

        public SubMenuController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpGet(RouteKey.GetAllSubMenu)]
        public async Task<IActionResult> GetActiveSubMenus(CancellationToken cancellationToken)
        {
            var subMenus = await _mediator.Send(new GetAllSubMenu.Query(), cancellationToken);
            return Ok(subMenus);
        }
    }
}
