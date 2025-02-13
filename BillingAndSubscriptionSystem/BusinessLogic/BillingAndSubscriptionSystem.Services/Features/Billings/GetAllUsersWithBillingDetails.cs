using BillingAndSubscriptionSystem.DataAccess;
using BillingAndSubscriptionSystem.Services.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BillingAndSubscriptionSystem.Services.Features.Billings
{
    public class GetAllUsersWithBillingDetails
    {
        public class Query : IRequest<ICollection<BillingDto>>
        {
            public BillingDto Billing { get; set; }

            public Query(BillingDto billing)
            {
                Billing = billing;
            }
        }

        public class Handler : IRequestHandler<Query, ICollection<BillingDto>>
        {
            private readonly UnitOfWork _unitOfWork;
            private readonly ILogger<GetAllUsersWithBillingDetails> _logger;

            public Handler(UnitOfWork unitOfWork, ILogger<GetAllUsersWithBillingDetails> logger)
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
                    ValidateRequest(request.Billing);
                    var billingDetails =
                        await _unitOfWork.BillingRepository.GetAllUsersWithBillingDetails(
                            cancellationToken
                        );
                    var details = billingDetails.Select(MapBilling).ToList();
                    return details;
                }
                catch (Exception exception)
                {
                    _logger.LogError(
                        exception,
                        "Error fetching users with billing details:{Exception}",
                        exception.Message
                    );
                    throw new InvalidOperationException(
                        "Error fetching users with billing details"
                    );
                }
            }

            private void ValidateRequest(BillingDto billing)
            {
                if (billing.UserId <= 0)
                {
                    throw new InvalidOperationException("Invalid user id");
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
