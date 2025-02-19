using BillingAndSubscriptionSystem.Core.Exceptions;
using BillingAndSubscriptionSystem.DataAccess;
using BillingAndSubscriptionSystem.Services.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BillingAndSubscriptionSystem.Services.Features.Users
{
    public class GetUserById
    {
        public class Query : IRequest<UserDto?>
        {
            public int UserId { get; }

            public Query(int userId)
            {
                UserId = userId;
            }
        }

        public class Handler : IRequestHandler<Query, UserDto?>
        {
            private readonly UnitOfWork _unitOfWork;
            private readonly ILogger<GetUserById> _logger;

            public Handler(UnitOfWork unitOfWork, ILogger<GetUserById> logger)
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
            }

            public async Task<UserDto?> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    var user = await _unitOfWork.UserRepository.GetUserByIdAsync(
                        request.UserId,
                        cancellationToken
                    );

                    if (user == null)
                    {
                        return null;
                    }
                    var userDto = new UserDto
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        Phone = user.Phone,
                        Password = user.Password,
                        Role = user.RoleId == 1 ? "Admin" : "User",
                    };
                    return userDto;
                }
                catch (Exception exception)
                {
                    _logger.LogError(
                        exception,
                        "Error fetching user with ID {UserId}: {Message}",
                        request.UserId,
                        exception.Message
                    );
                    throw new CustomException(
                        $"Error fetching user with ID {request.UserId}.",
                        exception
                    );
                }
            }
        }
    }
}
