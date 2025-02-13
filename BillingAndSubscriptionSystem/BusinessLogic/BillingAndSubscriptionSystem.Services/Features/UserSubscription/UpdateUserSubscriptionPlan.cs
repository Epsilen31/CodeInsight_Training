using BillingAndSubscriptionSystem.DataAccess;
using BillingAndSubscriptionSystem.Entities.Entities;
using BillingAndSubscriptionSystem.Services.DTOs;
using MediatR;

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

            public Handler(UnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Unit> Handle(Query request, CancellationToken cancellationToken)
            {
                ValidateRequest(request.Subscription);
                var existingSubscription =
                    await _unitOfWork.UserSubscriptionRepository.GetUserSubscriptionAsync(
                        request.Subscription.SubscriptionId
                    );
                if (existingSubscription == null)
                {
                    throw new InvalidOperationException("Subscription not found");
                }

                var subscription = MapSubscription(request.Subscription);
                await _unitOfWork.UserSubscriptionRepository.UpdateSubscriptionAsync(subscription);
                return Unit.Value;
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
