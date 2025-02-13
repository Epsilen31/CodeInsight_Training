using BillingAndSubscriptionSystem.DataAccess;
using BillingAndSubscriptionSystem.Services.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

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
            private readonly ILogger<GetOverduePayments> _logger;

            public Handler(UnitOfWork unitOfWork, ILogger<GetOverduePayments> logger)
            {
                _logger = logger;
                _unitOfWork = unitOfWork;
            }

            public async Task<ICollection<PaymentDto>> Handle(
                Query request,
                CancellationToken cancellationToken
            )
            {
                try
                {
                    var overduePayments = await _unitOfWork.PaymentRepository.OverduePayments(
                        cancellationToken
                    );

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
                catch (Exception exception)
                {
                    _logger.LogError(
                        exception,
                        "Error fetching overdue payments: {Exception}",
                        exception.Message
                    );
                    throw new InvalidOperationException("Error fetching overdue payments");
                }
            }
        }
    }
}
