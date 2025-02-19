using BillingAndSubscriptionSystem.Core.Exceptions;
using BillingAndSubscriptionSystem.DataAccess;
using BillingAndSubscriptionSystem.Entities.Entities;
using BillingAndSubscriptionSystem.Services.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BillingAndSubscriptionSystem.Services.Features.Authentication
{
    public class RegisterUser
    {
        public class Command : IRequest<UserDto>
        {
            public UserDto User { get; set; }

            public Command(UserDto user)
            {
                User = user;
            }
        }

        public class Handler : IRequestHandler<Command, UserDto>
        {
            private readonly UnitOfWork _unitOfWork;
            private readonly ILogger<RegisterUser> _logger;

            public Handler(UnitOfWork unitOfWork, ILogger<RegisterUser> logger)
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
            }

            public async Task<UserDto> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    if (
                        string.IsNullOrEmpty(request.User.Email)
                        || string.IsNullOrEmpty(request.User.Password)
                    )
                    {
                        _logger.LogError("Email and password are required.");
                        throw new CustomException("Email and password are required.", null);
                    }

                    var existingUser = (
                        await _unitOfWork.UserRepository.GetAllUsersAsync(cancellationToken)
                    ).FirstOrDefault(user => user.Email == request.User.Email);

                    if (existingUser != null)
                    {
                        _logger.LogWarning("User with the given email already exists.");
                        throw new CustomException(
                            "User with the given email already exists.",
                            null
                        );
                    }

                    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.User.Password);
                    var newUser = new User
                    {
                        Email = request.User.Email,
                        Password = hashedPassword,
                        Name = request.User.Name,
                        Phone = request.User.Phone,
                        Role = request.User.Role ?? new Role { RoleName = "Admin" },
                    };

                    await _unitOfWork.UserRepository.AddUserAsync(newUser, cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return new UserDto { Email = newUser.Email };
                }
                catch (Exception exception)
                {
                    _logger.LogError(
                        exception,
                        "Error occurred while registering the user {Message}",
                        exception.Message
                    );
                    throw new CustomException(
                        "Error occurred while registering the user.",
                        exception
                    );
                }
            }
        }
    }
}
