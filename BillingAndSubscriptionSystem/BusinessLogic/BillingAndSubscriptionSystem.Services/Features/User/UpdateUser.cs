using BillingAndSubscriptionSystem.Core.Exceptions;
using BillingAndSubscriptionSystem.DataAccess;
using BillingAndSubscriptionSystem.Services.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BillingAndSubscriptionSystem.Services.Features.Users
{
    public class UpdateUser
    {
        public class Query : IRequest<UserDto?>
        {
            public UserDto User { get; }

            public Query(UserDto user)
            {
                User = user;
            }
        }

        public class Handler : IRequestHandler<Query, UserDto?>
        {
            private readonly UnitOfWork _unitOfWork;
            private readonly ILogger<UpdateUser> _logger;

            public Handler(UnitOfWork unitOfWork, ILogger<UpdateUser> logger)
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
            }

            public async Task<UserDto?> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    var existingUser = await _unitOfWork.UserRepository.GetUserByIdAsync(
                        request.User.Id
                    );
                    if (existingUser == null)
                    {
                        return null;
                    }

                    existingUser.Name = request.User.Name;
                    existingUser.Email = request.User.Email;
                    existingUser.Phone = request.User.Phone;

                    await _unitOfWork.UserRepository.UpdateUserAsync(existingUser);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    var updatedUserDto = new UserDto
                    {
                        Id = existingUser.Id,
                        Name = existingUser.Name,
                        Email = existingUser.Email,
                        Phone = existingUser.Phone,
                    };

                    return updatedUserDto;
                }
                catch (Exception exception)
                {
                    _logger.LogError(
                        exception,
                        "Error updating user {UserId}: {Message}",
                        request.User.Id,
                        exception.Message
                    );
                    throw new CustomException(
                        $"Error updating user with ID {request.User.Id}.",
                        exception
                    );
                }
            }
        }
    }
}
