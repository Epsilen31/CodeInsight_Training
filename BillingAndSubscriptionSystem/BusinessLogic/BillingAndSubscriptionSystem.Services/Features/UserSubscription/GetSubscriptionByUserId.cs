using BillingAndSubscriptionSystem.DataAccess;
using BillingAndSubscriptionSystem.Entities.Entities;
using BillingAndSubscriptionSystem.Services.DTOs;
using MediatR;

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

            public Handler(UnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<ICollection<SubscriptionDto>> Handle(
                Query request,
                CancellationToken cancellationToken
            )
            {
                ValidateRequest(request.Subscription);
                var existingSubscription =
                    await _unitOfWork.UserSubscriptionRepository.GetUserSubscriptionAsync(
                        request.Subscription.UserId
                    );
                if (existingSubscription == null)
                {
                    throw new InvalidOperationException("Subscription not found");
                }

                var subscription = MapSubscription(request.Subscription);
                var subscriptionData =
                    await _unitOfWork.UserSubscriptionRepository.GetUserSubscriptionAsync(
                        subscription.UserId
                    );
                return [MapSubscriptionDto(subscriptionData)];
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
