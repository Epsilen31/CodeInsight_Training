using BillingAndSubscriptionSystem.Core.Exceptions;
using BillingAndSubscriptionSystem.DataAccess.Contracts;
using BillingAndSubscriptionSystem.Services.DTOs;
using MapsterMapper;
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
            private readonly IMapper _mapper;
            private readonly ILogger<Handler> _logger;

            public Handler(IUnitOfWork unitOfWork, ILogger<Handler> logger, IMapper mapper)
            {
                _mapper = mapper;
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

                    var newUser = _mapper.Map<BillingAndSubscriptionSystem.Entities.Entities.User>(
                        request.User
                    );

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
