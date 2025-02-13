using BillingAndSubscriptionSystem.DataAccess;
using BillingAndSubscriptionSystem.Services.DTOs;
using MediatR;

namespace BillingAndSubscriptionSystem.Services.Features.Billing
{
    public class UpdateBillingDetails
    {
        public class Query : IRequest<Unit>
        {
            public BillingDto Billing { get; set; }

            public Query(BillingDto billing)
            {
                Billing = billing;
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
                ValidateRequest(request.Billing);
                var billingEntity = MapBilling(request.Billing);
                await _unitOfWork.BillingRepository.UpdateBillingDetails(billingEntity);
                return Unit.Value;
            }

            private void ValidateRequest(BillingDto billing)
            {
                if (billing.UserId <= 0)
                {
                    throw new InvalidOperationException("Invalid user id");
                }
            }

            private Entities.Entities.Billing MapBilling(BillingDto billing)
            {
                return new Entities.Entities.Billing
                {
                    UserId = billing.UserId,
                    BillingAddress = billing.BillingAddress,
                    PaymentMethod = billing.PaymentMethod,
                };
            }
        }
    }
}
