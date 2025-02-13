using BillingAndSubscriptionSystem.DataAccess;
using BillingAndSubscriptionSystem.Entities.Entities;
using BillingAndSubscriptionSystem.Entities.Enums;
using BillingAndSubscriptionSystem.Services.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

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
            public readonly ILogger<PaymentProcess> _logger;

            public Handler(UnitOfWork unitOfWork, ILogger<PaymentProcess> logger)
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
            }

            public async Task<Unit> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    var createPaymentDetails = MapPayments(request.Payment);
                    await _unitOfWork.PaymentRepository.ProcessPaymentAsync(
                        createPaymentDetails,
                        cancellationToken
                    );
                    return Unit.Value;
                }
                catch (Exception exception)
                {
                    _logger.LogError(
                        exception,
                        "An error occurred while processing payment: {Exception}",
                        exception.Message
                    );
                    throw new InvalidOperationException("Error processing payment");
                }
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
