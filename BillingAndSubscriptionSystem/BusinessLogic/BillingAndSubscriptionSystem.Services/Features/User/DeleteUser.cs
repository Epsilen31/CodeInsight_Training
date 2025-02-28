using BillingAndSubscriptionSystem.Core.Exceptions;
using BillingAndSubscriptionSystem.DataAccess.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BillingAndSubscriptionSystem.Services.Features.Users
{
    public class DeleteUser
    {
        public class Command : IRequest<bool>
        {
            public int UserId { get; }

            public Command(int userId)
            {
                UserId = userId;
            }
        }

        public class Handler : IRequestHandler<Command, bool>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ILogger<Handler> _logger;

            public Handler(IUnitOfWork unitOfWork, ILogger<Handler> logger)
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var success = await _unitOfWork.UserRepository.DeleteUserAsync(
                        request.UserId,
                        cancellationToken
                    );

                    if (success)
                    {
                        await _unitOfWork.SaveChangesAsync(cancellationToken);
                    }
                    return success;
                }
                catch (Exception exception)
                {
                    _logger.LogError(
                        exception,
                        "Error deleting user with ID {UserId}: {Message}",
                        request.UserId,
                        exception.Message
                    );
                    throw new CustomException(
                        $"Error deleting user with ID {request.UserId}.",
                        exception
                    );
                }
            }
        }
    }
}
