using BillingAndSubscriptionSystem.Core.Exceptions;
using BillingAndSubscriptionSystem.DataAccess.Contracts;
using BillingAndSubscriptionSystem.Services.DTOs;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BillingAndSubscriptionSystem.Services.Features.Users
{
    public class UpdateUser
    {
        public class Command : IRequest<UserDto?>
        {
            public UserDto User { get; }

            public Command(UserDto user)
            {
                User = user;
            }
        }

        public class Handler : IRequestHandler<Command, UserDto?>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private readonly ILogger<Handler> _logger;

            public Handler(IUnitOfWork unitOfWork, ILogger<Handler> logger, IMapper mapper)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _logger = logger;
            }

            public async Task<UserDto?> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var existingUser = await _unitOfWork.UserRepository.GetUserByIdAsync(
                        request.User.Id,
                        cancellationToken
                    );

                    if (existingUser == null)
                    {
                        return null;
                    }

                    existingUser.Name = request.User.Name;

                    existingUser.Email = request.User.Email;

                    existingUser.Phone = request.User.Phone;

                    existingUser.Password = request.User.Password;

                    await _unitOfWork.UserRepository.UpdateUserAsync(
                        existingUser,
                        cancellationToken
                    );
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    var updatedUserDto = _mapper.Map<UserDto>(existingUser);
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
