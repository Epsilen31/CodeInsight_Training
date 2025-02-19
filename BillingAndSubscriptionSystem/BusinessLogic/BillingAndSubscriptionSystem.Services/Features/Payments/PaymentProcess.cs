using BillingAndSubscriptionSystem.Core.Exceptions;
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
        public class Command : IRequest<PaymentDto>
        {
            public PaymentDto Payment { get; }

            public Command(PaymentDto payment)
            {
                Payment = payment;
            }
        }

        public class Handler : IRequestHandler<Command, PaymentDto>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ILogger<PaymentProcess> _logger;

            public Handler(IUnitOfWork unitOfWork, ILogger<PaymentProcess> logger)
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
            }

            public async Task<PaymentDto> Handle(
                Command request,
                CancellationToken cancellationToken
            )
            {
                try
                {
                    var paymentEntity = MapPayments(request.Payment);
                    await _unitOfWork.PaymentRepository.ProcessPaymentAsync(
                        paymentEntity,
                        cancellationToken
                    );
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return new PaymentDto
                    {
                        Id = paymentEntity.Id,
                        SubscriptionId = paymentEntity.SubscriptionId,
                        Amount = paymentEntity.Amount,
                        PaymentDate = paymentEntity.PaymentDate,
                        PaymentStatus = paymentEntity.PaymentStatus,
                    };
                }
                catch (Exception exception)
                {
                    _logger.LogError(
                        exception,
                        "Error processing payment for Subscription ID: {SubscriptionId}",
                        request.Payment.SubscriptionId
                    );
                    throw new CustomException("Error processing payment.", exception);
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
