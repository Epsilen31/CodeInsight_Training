using BillingAndSubscriptionSystem.Services.DTOs;
using BillingAndSubscriptionSystem.Services.Features.Users;
using BillingAndSubscriptionSystem.WebApi.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BillingAndSubscriptionSystem.WebApi.Controllers.Users
{
    [Authorize]
    [ApiController]
    [Route(RouteKey.UserRoute)]
    public class BillingAndSubscriptionSystem : BaseController
    {
        private readonly IMediator _mediator;

        public BillingAndSubscriptionSystem(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(RouteKey.GetUsers)]
        public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
        {
            var users = await _mediator.Send(new GetAllUsers.Query(), cancellationToken);
            return Ok(new { Message = "Users retrieved successfully", Users = users });
        }

        [HttpGet(RouteKey.GetUserById)]
        public async Task<IActionResult> GetUserById(
            [FromRoute] int userId,
            CancellationToken cancellationToken
        )
        {
            var user = await _mediator.Send(new GetUserById.Query(userId), cancellationToken);
            if (user == null)
            {
                return NotFound(new { Message = $"User with ID {userId} not found" });
            }
            return Ok(new { Message = "User retrieved successfully", User = user });
        }

        [HttpPost(RouteKey.AddUser)]
        public async Task<IActionResult> AddUser(
            [FromBody] UserDto userDto,
            CancellationToken cancellationToken
        )
        {
            await _mediator.Send(new AddUser.Command(userDto), cancellationToken);
            return Ok(new { Message = "User added successfully" });
        }

        [HttpPut(RouteKey.UpdateUser)]
        public async Task<IActionResult> UpdateUser(
            int userId,
            [FromBody] UserDto userDto,
            CancellationToken cancellationToken
        )
        {
            userDto.Id = userId;
            var result = await _mediator.Send(new UpdateUser.Command(userDto), cancellationToken);

            if (result != null)
            {
                return Ok(new { Message = "User updated successfully" });
            }
            else
            {
                return BadRequest(new { Message = "Failed to update user" });
            }
        }

        [HttpDelete(RouteKey.DeleteUser)]
        public async Task<IActionResult> DeleteUser(
            [FromRoute] int userId,
            CancellationToken cancellationToken
        )
        {
            var result = await _mediator.Send(new DeleteUser.Command(userId), cancellationToken);

            if (result)
            {
                return Ok(new { Message = "User deleted successfully" });
            }
            else
            {
                return StatusCode(500);
            }
        }
    }
}
