using BillingAndSubscriptionSystem.Core.Exceptions;
using BillingAndSubscriptionSystem.DataAccess.Contracts;
using BillingAndSubscriptionSystem.Services.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BillingAndSubscriptionSystem.Services.Features.Billings
{
    public class GetAllUsersWithBillingDetails
    {
        public class Query : IRequest<ICollection<BillingDto>>
        {
            public int UserId { get; }

            public Query(int userId)
            {
                UserId = userId;
            }
        }

        public class Handler : IRequestHandler<Query, ICollection<BillingDto>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ILogger<GetAllUsersWithBillingDetails> _logger;

            public Handler(IUnitOfWork unitOfWork, ILogger<GetAllUsersWithBillingDetails> logger)
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
            }

            public async Task<ICollection<BillingDto>> Handle(
                Query request,
                CancellationToken cancellationToken
            )
            {
                try
                {
                    if (request.UserId <= 0)
                    {
                        throw new CustomException("Invalid user ID.", null);
                    }

                    var billingDetails =
                        await _unitOfWork.BillingRepository.GetAllUsersWithBillingDetails(
                            request.UserId,
                            cancellationToken
                        );

                    if (billingDetails == null || billingDetails.Count == 0)
                    {
                        return [];
                    }
                    return [.. billingDetails.Select(MapBilling)];
                }
                catch (Exception exception)
                {
                    _logger.LogError(
                        exception,
                        "Error fetching users with billing details: {ExceptionMessage}",
                        exception.Message
                    );
                    throw new CustomException(
                        "An error occurred while fetching users with billing details.",
                        exception
                    );
                }
            }

            private BillingDto MapBilling(Entities.Entities.Billing billing)
            {
                return new BillingDto
                {
                    UserId = billing.UserId,
                    BillingAddress = billing.BillingAddress,
                    PaymentMethod = billing.PaymentMethod,
                };
            }
        }
    }
}
