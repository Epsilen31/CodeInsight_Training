using BillingAndSubscriptionSystem.DataAccess;
using BillingAndSubscriptionSystem.Entities.Entities;
using BillingAndSubscriptionSystem.Services.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BillingAndSubscriptionSystem.Services.Features
{
    public class UpdateUserSubscriptionPlan
    {
        public class Query : IRequest<Unit>
        {
            public SubscriptionDto Subscription { get; set; }

            public Query(SubscriptionDto subscription)
            {
                Subscription = subscription;
            }
        }

        public class Handler : IRequestHandler<Query, Unit>
        {
            private readonly UnitOfWork _unitOfWork;
            private readonly ILogger<UpdateUserSubscriptionPlan> _logger;

            public Handler(UnitOfWork unitOfWork, ILogger<UpdateUserSubscriptionPlan> logger)
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
            }

            public async Task<Unit> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    ValidateRequest(request.Subscription);
                    var existingSubscription =
                        await _unitOfWork.UserSubscriptionRepository.GetUserSubscriptionAsync(
                            request.Subscription.SubscriptionId,
                            cancellationToken
                        );
                    if (existingSubscription == null)
                    {
                        throw new InvalidOperationException("Subscription not found");
                    }

                    var subscription = MapSubscription(request.Subscription);
                    await _unitOfWork.UserSubscriptionRepository.UpdateSubscriptionAsync(
                        subscription,
                        cancellationToken
                    );
                    return Unit.Value;
                }
                catch (Exception exception)
                {
                    _logger.LogError(
                        exception,
                        "Error updating subscription plan : {Exception}",
                        exception.Message
                    );
                    throw new InvalidOperationException("Error updating subscription plan");
                }
            }

            private void ValidateRequest(SubscriptionDto subscription)
            {
                if (subscription == null)
                {
                    throw new ArgumentNullException(nameof(subscription));
                }

                if (subscription.SubscriptionId <= 0)
                {
                    throw new InvalidOperationException("Invalid subscription id");
                }
            }

            private Subscription MapSubscription(SubscriptionDto subscription)
            {
                return new Subscription
                {
                    Id = subscription.SubscriptionId,
                    UserId = subscription.UserId,
                    PlanType = subscription.PlanType,
                    StartDate = subscription.StartDate,
                    EndDate = subscription.EndDate,
                    SubscriptionStatus =
                        subscription.SubscriptionStatus == 0
                            ? Entities.Enums.SubscriptionStatus.Active
                            : Entities.Enums.SubscriptionStatus.Inactive,
                };
            }
        }
    }
}
