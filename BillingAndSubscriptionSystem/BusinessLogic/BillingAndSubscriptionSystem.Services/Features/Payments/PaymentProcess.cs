using BillingAndSubscriptionSystem.Core.Exceptions;
using BillingAndSubscriptionSystem.DataAccess.Contracts;
using BillingAndSubscriptionSystem.Entities.Entities;
using BillingAndSubscriptionSystem.Services.DTOs;
using MapsterMapper;
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
            private readonly IMapper _mapper;
            private readonly ILogger<Handler> _logger;

            public Handler(IUnitOfWork unitOfWork, ILogger<Handler> logger, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
                _mapper = mapper;
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

                    return _mapper.Map<PaymentDto>(paymentEntity);
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
                return _mapper.Map<Payment>(payment);
            }
        }
    }
}
