using BillingAndSubscriptionSystem.DataAccess;
using BillingAndSubscriptionSystem.Entities.Entities;
using BillingAndSubscriptionSystem.Entities.Enums;
using BillingAndSubscriptionSystem.Services.DTOs;
using MediatR;

namespace BillingAndSubscriptionSystem.Services.Features.Payments
{
    public class PaymentProcess
    {
        public class Query : IRequest<Unit>
        {
            public PaymentDto Payment { get; set; }

            public Query(PaymentDto payment)
            {
                Payment = payment ?? throw new ArgumentNullException(nameof(payment));
            }
        }

        public class Handler : IRequestHandler<Query, Unit>
        {
            private readonly UnitOfWork _unitOfWork;

            public Handler(UnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            }

            public async Task<Unit> Handle(Query request, CancellationToken cancellationToken)
            {
                var createPaymentDetails = MapPayments(request.Payment);
                await _unitOfWork.PaymentRepository.ProcessPaymentAsync(createPaymentDetails);
                return Unit.Value;
            }

            private Payment MapPayments(PaymentDto payment)
            {
                return new Payment
                {
                    Id = payment.Id,
                    SubscriptionId = payment.SubscriptionId,
                    Amount = payment.Amount,
                    PaymentDate = payment.PaymentDate,
                    PaymentStatus =
                        payment.PaymentStatus == PaymentStatus.Paid
                            ? PaymentStatus.Paid
                            : PaymentStatus.Overdue,
                };
            }
        }
    }
}
