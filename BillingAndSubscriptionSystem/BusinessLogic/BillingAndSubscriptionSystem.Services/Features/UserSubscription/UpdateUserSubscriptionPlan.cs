using BillingAndSubscriptionSystem.Core.Exceptions;
using BillingAndSubscriptionSystem.DataAccess;
using BillingAndSubscriptionSystem.DataAccess.Contracts;
using BillingAndSubscriptionSystem.Services.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BillingAndSubscriptionSystem.Services.Features
{
    public class UpdateUserSubscriptionPlan
    {
        public class Query : IRequest<SubscriptionDto>
        {
            public SubscriptionDto Subscription { get; set; }

            public Query(SubscriptionDto subscription)
            {
                Subscription = subscription;
            }
        }

        public class Handler : IRequestHandler<Query, SubscriptionDto>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ILogger<UpdateUserSubscriptionPlan> _logger;

            public Handler(IUnitOfWork unitOfWork, ILogger<UpdateUserSubscriptionPlan> logger)
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
                            request.Subscription.SubscriptionId,
                            cancellationToken
                        );

                    if (existingSubscription == null)
                    {
                        throw new CustomException(
                            $"Subscription with ID {request.Subscription.SubscriptionId} not found.",
                            null
                        );
                    }

                    existingSubscription.PlanType = request.Subscription.PlanType;
                    existingSubscription.StartDate = request.Subscription.StartDate;
                    existingSubscription.EndDate = request.Subscription.EndDate;
                    existingSubscription.SubscriptionStatus =
                        request.Subscription.SubscriptionStatus == 0
                            ? Entities.Enums.SubscriptionStatus.Active
                            : Entities.Enums.SubscriptionStatus.Inactive;

                    await _unitOfWork.UserSubscriptionRepository.UpdateSubscriptionAsync(
                        existingSubscription,
                        cancellationToken
                    );

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return new SubscriptionDto
                    {
                        SubscriptionId = existingSubscription.Id,
                        UserId = existingSubscription.UserId,
                        PlanType = existingSubscription.PlanType,
                        StartDate = existingSubscription.StartDate,
                        EndDate = existingSubscription.EndDate,
                        SubscriptionStatus = existingSubscription.SubscriptionStatus,
                    };
                }
                catch (Exception exception)
                {
                    _logger.LogError(
                        exception,
                        "Error updating subscription plan: {Exception}",
                        exception.Message
                    );
                    throw new CustomException(
                        $"Error updating subscription plan: {exception.Message}",
                        null
                    );
                }
            }

            private void ValidateRequest(SubscriptionDto subscription)
            {
                if (subscription == null || subscription.SubscriptionId <= 0)
                {
                    throw new CustomException(
                        "Invalid subscription ID. Please provide a valid ID.",
                        null
                    );
                }
            }
        }
    }
}
