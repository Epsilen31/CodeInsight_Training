using BillingAndSubscriptionSystem.DataAccess;
using BillingAndSubscriptionSystem.Entities.Entities;
using BillingAndSubscriptionSystem.Services.DTOs;
using MediatR;

namespace BillingAndSubscriptionSystem.Services.Features
{
    public class CreateUserSubscriptionPlan
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
                try
                {
                    ValidateRequest(request.Subscription);

                    var existingSubscription =
                        await _unitOfWork.UserSubscriptionRepository.GetUserSubscriptionAsync(
                            request.Subscription.UserId
                        );

                    if (existingSubscription != null)
                    {
                        throw new InvalidOperationException("User already has a subscription plan");
                    }

                    var subscription = MapSubscription(request.Subscription);
                    await _unitOfWork.UserSubscriptionRepository.CreateUserSubscriptionAsync(
                        subscription
                    );
                    return Unit.Value;
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(ex.Message);
                }
            }

            private void ValidateRequest(SubscriptionDto subscription)
            {
                if (subscription == null)
                {
                    throw new InvalidOperationException(nameof(subscription));
                }
            }

            private Subscription MapSubscription(SubscriptionDto subscription)
            {
                return new Subscription
                {
                    UserId = subscription.UserId,
                    PlanType = subscription.PlanType,
                    StartDate = subscription.StartDate,
                    EndDate = subscription.EndDate,
                };
            }
        }
    }
}
