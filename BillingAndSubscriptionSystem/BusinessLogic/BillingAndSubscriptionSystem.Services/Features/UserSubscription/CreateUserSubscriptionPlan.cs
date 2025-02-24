using BillingAndSubscriptionSystem.Core.Exceptions;
using BillingAndSubscriptionSystem.DataAccess;
using BillingAndSubscriptionSystem.DataAccess.Contracts;
using BillingAndSubscriptionSystem.Entities.Entities;
using BillingAndSubscriptionSystem.Services.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BillingAndSubscriptionSystem.Services.Features
{
    public class CreateUserSubscriptionPlan
    {
        public class Query : IRequest<SubscriptionDto>
        {
            public SubscriptionDto Subscription { get; }

            public Query(SubscriptionDto subscription)
            {
                Subscription =
                    subscription ?? throw new ArgumentNullException(nameof(subscription));
            }
        }

        public class Handler : IRequestHandler<Query, SubscriptionDto>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ILogger<CreateUserSubscriptionPlan> _logger;

            public Handler(IUnitOfWork unitOfWork, ILogger<CreateUserSubscriptionPlan> logger)
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
            }

            public async Task<SubscriptionDto> Handle(
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

                    if (existingSubscription != null)
                    {
                        throw new CustomException("User already has a subscription plan.", null);
                    }

                    var subscriptionEntity = MapSubscription(request.Subscription);
                    await _unitOfWork.UserSubscriptionRepository.CreateUserSubscriptionAsync(
                        subscriptionEntity,
                        cancellationToken
                    );
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return new SubscriptionDto
                    {
                        UserId = subscriptionEntity.UserId,
                        PlanType = subscriptionEntity.PlanType,
                        StartDate = subscriptionEntity.StartDate,
                        EndDate = subscriptionEntity.EndDate,
                    };
                }
                catch (Exception exception)
                {
                    _logger.LogError(
                        exception,
                        "Error creating subscription plan for User ID {UserId}: {Message}",
                        request.Subscription.UserId,
                        exception.Message
                    );
                    throw new CustomException("Error creating subscription plan.", exception);
                }
            }

            private void ValidateRequest(SubscriptionDto subscription)
            {
                if (subscription.UserId <= 0)
                {
                    throw new CustomException("Invalid User ID.", null);
                }
                if (subscription.StartDate >= subscription.EndDate)
                {
                    throw new CustomException(
                        "Subscription Start Date must be before End Date.",
                        null
                    );
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
