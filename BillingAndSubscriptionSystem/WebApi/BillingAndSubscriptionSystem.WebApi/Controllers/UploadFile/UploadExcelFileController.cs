using System.Security.Claims;
using BillingAndSubscriptionSystem.Services.Features.User;
using BillingAndSubscriptionSystem.WebApi.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BillingAndSubscriptionSystem.WebApi.Controllers.UploadFile
{
    [ApiController]
    [Route(RouteKey.UploadFileRoute)]
    public class UploadExcelFileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UploadExcelFileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost(RouteKey.UploadExcelFile)]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
        {
            var userId =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value
                ?? User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID is missing.");
            }
            var result = await _mediator.Send(
                new UploadExcelUsers.Command { File = file, UserId = userId }
            );
            return Ok(new { message = result });
        }
    }
}
