using BillingAndSubscriptionSystem.Core.Exceptions;
using BillingAndSubscriptionSystem.DataAccess.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BillingAndSubscriptionSystem.Services.Features.UserSubscription
{
    public class DeleteUserSubscription
    {
        public class Command : IRequest<bool>
        {
            public int SubscriptionId { get; }

            public Command(int subscriptionId)
            {
                SubscriptionId = subscriptionId;
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
                    Console.WriteLine($"{request.SubscriptionId}");
                    var success =
                        await _unitOfWork.UserSubscriptionRepository.DeleteUserSubscriptionAsync(
                            request.SubscriptionId,
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
                        "Error deleting user subscription with subscriptionId {SubscriptionId}: {Message}.",
                        request.SubscriptionId,
                        exception.Message
                    );

                    return false; // Return false to indicate failure
                }
            }
        }
    }
}
