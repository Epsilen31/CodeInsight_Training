using BillingAndSubscriptionSystem.Core.Exceptions;
using BillingAndSubscriptionSystem.DataAccess;
using BillingAndSubscriptionSystem.Entities.Entities;
using BillingAndSubscriptionSystem.Services.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BillingAndSubscriptionSystem.Services.Features.Users
{
    public class AddUser
    {
        public class Command : IRequest<Unit>
        {
            public UserDto User { get; }

            public Command(UserDto user)
            {
                User = user;
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ILogger<AddUser> _logger;

            public Handler(IUnitOfWork unitOfWork, ILogger<AddUser> logger)
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    if (
                        string.IsNullOrEmpty(request.User.Email)
                        || string.IsNullOrEmpty(request.User.Password)
                    )
                    {
                        _logger.LogWarning("Email and password are required.");
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
                        Name = request.User.Name,
                        Email = request.User.Email,
                        Phone = request.User.Phone,
                        Password = hashedPassword,
                        Role = request.User.Role ?? new Role { RoleName = "Admin" },
                    };

                    await _unitOfWork.UserRepository.AddUserAsync(newUser, cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    return Unit.Value;
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Error adding user: {Message}", exception.Message);
                    throw new CustomException("Error adding user.", exception);
                }
            }
        }
    }
}
