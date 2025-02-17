using BillingAndSubscriptionSystem.Core.Exceptions;
using BillingAndSubscriptionSystem.DataAccess;
using BillingAndSubscriptionSystem.Entities.Entities;
using BillingAndSubscriptionSystem.Services.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BillingAndSubscriptionSystem.Services.Features.UserSubscription
{
    public class GetSubscriptionByUserId
    {
        public class Query : IRequest<List<SubscriptionDto>>
        {
            public int UserId { get; }

            public Query(int userId)
            {
                if (userId <= 0)
                {
                    throw new ArgumentException("Invalid User ID", nameof(userId));
                }
                UserId = userId;
            }
        }

        public class Handler : IRequestHandler<Query, List<SubscriptionDto>>
        {
            private readonly UnitOfWork _unitOfWork;
            private readonly ILogger<GetSubscriptionByUserId> _logger;

            public Handler(UnitOfWork unitOfWork, ILogger<GetSubscriptionByUserId> logger)
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
            }

            public async Task<List<SubscriptionDto>> Handle(
                Query request,
                CancellationToken cancellationToken
            )
            {
                try
                {
                    var subscription =
                        await _unitOfWork.UserSubscriptionRepository.GetUserSubscriptionAsync(
                            request.UserId,
                            cancellationToken
                        );

                    if (subscription == null)
                    {
                        throw new CustomException(
                            $"No subscription found for User ID {request.UserId}",
                            null
                        );
                    }

                    return [MapSubscriptionDto(subscription)];
                }
                catch (Exception exception)
                {
                    _logger.LogError(
                        exception,
                        "Error fetching subscription for User ID {UserId}: {Message}",
                        request.UserId,
                        exception.Message
                    );
                    throw new CustomException("Error getting user subscription.", exception);
                }
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
