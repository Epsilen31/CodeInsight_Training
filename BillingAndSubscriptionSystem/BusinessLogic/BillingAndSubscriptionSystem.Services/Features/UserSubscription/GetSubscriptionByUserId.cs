using BillingAndSubscriptionSystem.DataAccess;
using BillingAndSubscriptionSystem.Entities.Entities;
using BillingAndSubscriptionSystem.Services.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BillingAndSubscriptionSystem.Services.Features.UserSubscription
{
    public class GetSubscriptionByUserId
    {
        public class Query : IRequest<ICollection<SubscriptionDto>>
        {
            public SubscriptionDto Subscription { get; set; }

            public Query(SubscriptionDto subscription)
            {
                Subscription = subscription;
            }
        }

        public class Handler : IRequestHandler<Query, ICollection<SubscriptionDto>>
        {
            private readonly UnitOfWork _unitOfWork;
            private readonly ILogger<GetSubscriptionByUserId> _logger;

            public Handler(UnitOfWork unitOfWork, ILogger<GetSubscriptionByUserId> logger)
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
            }

            public async Task<ICollection<SubscriptionDto>> Handle(
                Query request,
                CancellationToken cancellationToken
            )
            {
                try
                {
                    ValidateRequest(request.Subscription);
                    var existingSubscription =
                        await _unitOfWork.UserSubscriptionRepository.GetUserSubscriptionAsync(
                            request.Subscription.UserId,
                            cancellationToken
                        );
                    if (existingSubscription == null)
                    {
                        throw new InvalidOperationException("Subscription not found");
                    }

                    var subscription = MapSubscription(request.Subscription);
                    var subscriptionData =
                        await _unitOfWork.UserSubscriptionRepository.GetUserSubscriptionAsync(
                            subscription.UserId,
                            cancellationToken
                        );
                    return [MapSubscriptionDto(subscriptionData)];
                }
                catch (Exception exception)
                {
                    _logger.LogError(
                        exception,
                        "An error occurred while getting user subscription :{Exception}",
                        exception.Message
                    );
                    throw new InvalidOperationException("Error getting user subscription");
                }
            }

            private void ValidateRequest(SubscriptionDto subscription)
            {
                if (subscription == null)
                {
                    throw new ArgumentNullException(nameof(subscription));
                }

                if (subscription.UserId <= 0)
                {
                    throw new InvalidOperationException("Invalid user id");
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
                    SubscriptionStatus = subscription.SubscriptionStatus,
                };
            }

            private SubscriptionDto MapSubscriptionDto(Subscription subscription)
            {
                return new SubscriptionDto
                {
                    SubscriptionId = subscription.Id,
                    UserId = subscription.UserId,
                    PlanType = subscription.PlanType,
                    StartDate = subscription.StartDate,
                    EndDate = subscription.EndDate,
                    SubscriptionStatus = subscription.SubscriptionStatus,
                };
            }
        }
    }
}
