using BillingAndSubscriptionSystem.DataAccess;
using BillingAndSubscriptionSystem.Services.DTOs;
using MediatR;

namespace BillingAndSubscriptionSystem.Services.Features.Billings
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

        public Handler(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ICollection<BillingDto>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            ValidateRequest(request.Billing);
            var billingDetails =
                await _unitOfWork.BillingRepository.GetAllUsersWithBillingDetails();
            var details = billingDetails.Select(MapBilling).ToList();
            return details;
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
