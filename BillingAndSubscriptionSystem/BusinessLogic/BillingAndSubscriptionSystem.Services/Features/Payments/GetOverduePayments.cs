using BillingAndSubscriptionSystem.DataAccess;
using BillingAndSubscriptionSystem.Services.DTOs;
using MediatR;

namespace BillingAndSubscriptionSystem.Services.Features.Payments
{
    public class GetOverduePayments
    {
        public class Query : IRequest<ICollection<PaymentDto>>
        {
            public PaymentDto Payment { get; set; }

            public Query(PaymentDto payment)
            {
                Payment = payment;
            }
        }

        public class Handler : IRequestHandler<Query, ICollection<PaymentDto>>
        {
            private readonly UnitOfWork _unitOfWork;

            public Handler(UnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<ICollection<PaymentDto>> Handle(
                Query request,
                CancellationToken cancellationToken
            )
            {
                var overduePayments = await _unitOfWork.PaymentRepository.OverduePayments();

                if (overduePayments != null)
                {
                    return
                    [
                        .. overduePayments.Select(payment => new PaymentDto
                        {
                            Id = payment.Id,
                            SubscriptionId = payment.SubscriptionId,
                            Amount = payment.Amount,
                            PaymentDate = payment.PaymentDate,
                            PaymentStatus = payment.PaymentStatus,
                        }),
                    ];
                }
                else
                {
                    return [];
                }
            }
        }
    }
}
