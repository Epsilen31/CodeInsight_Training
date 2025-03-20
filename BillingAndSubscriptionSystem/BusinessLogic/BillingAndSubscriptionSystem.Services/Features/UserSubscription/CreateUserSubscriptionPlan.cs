using BillingAndSubscriptionSystem.Core.Exceptions;
using BillingAndSubscriptionSystem.DataAccess.Contracts;
using BillingAndSubscriptionSystem.Entities.Entities;
using BillingAndSubscriptionSystem.Services.DTOs;
using MapsterMapper;
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
            private readonly IMapper _mapper;
            private readonly ILogger<Handler> _logger;

            public Handler(IUnitOfWork unitOfWork, ILogger<Handler> logger, IMapper mapper)
            {
                _mapper = mapper;
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
                    _logger.LogInformation(
                        "Received subscription request for User ID: {UserId}",
                        request.Subscription.UserId
                    );
                    ValidateRequest(request.Subscription);

                    var userExists = await _unitOfWork.UserRepository.ExistsAsync(
                        request.Subscription.UserId
                    );
                    if (!userExists)
                    {
                        _logger.LogError(
                            "User ID {UserId} does not exist in the database.",
                            request.Subscription.UserId
                        );
                        throw new CustomException("Invalid User ID.");
                    }

                    var existingSubscription =
                        await _unitOfWork.UserSubscriptionRepository.GetUserSubscriptionAsync(
                            request.Subscription.UserId,
                            cancellationToken
                        );

                    if (existingSubscription != null)
                    {
                        throw new CustomException("User already has a subscription plan.");
                    }

                    // Map DTO to Entity
                    var subscriptionEntity = MapSubscription(request.Subscription);

                    await _unitOfWork.UserSubscriptionRepository.CreateUserSubscriptionAsync(
                        subscriptionEntity,
                        cancellationToken
                    );
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    _logger.LogInformation(
                        "Subscription successfully created for User ID: {UserId}",
                        request.Subscription.UserId
                    );

                    return _mapper.Map<SubscriptionDto>(subscriptionEntity);
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
                _logger.LogInformation(
                    "Validating subscription request for User ID: {UserId}",
                    subscription.UserId
                );

                if (subscription.UserId <= 0)
                {
                    throw new CustomException("Invalid User ID.");
                }
                if (subscription.StartDate >= subscription.EndDate)
                {
                    throw new CustomException("Subscription Start Date must be before End Date.");
                }
            }

            private Subscription MapSubscription(SubscriptionDto subscription)
            {
                return _mapper.Map<Subscription>(subscription);
            }
        }
    }
}
