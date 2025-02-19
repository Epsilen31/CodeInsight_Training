using BillingAndSubscriptionSystem.Core.Exceptions;
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
            public int UserId { get; }

            public Query(int userId)
            {
                UserId = userId;
            }
        }

        public class Handler : IRequestHandler<Query, ICollection<PaymentDto>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ILogger<GetOverduePayments> _logger;

            public Handler(IUnitOfWork unitOfWork, ILogger<GetOverduePayments> logger)
            {
                _IUnitOfWork = unitOfWork;
                _logger = logger;
            }

            public async Task<ICollection<PaymentDto>> Handle(
                Query request,
                CancellationToken cancellationToken
            )
            {
                try
                {
                    var overduePayments = await _unitOfWork.PaymentRepository.OverduePaymentsAsync(
                        cancellationToken
                    );

                    if (overduePayments == null || overduePayments.Count == 0)
                    {
                        _logger.LogWarning(
                            "No overdue payments found for UserId: {UserId}",
                            request.UserId
                        );
                        return [];
                    }

                    var paymentDetail = overduePayments
                        .Select(payment => new PaymentDto
                        {
                            Id = payment.Id,
                            SubscriptionId = payment.SubscriptionId,
                            Amount = payment.Amount,
                            PaymentDate = payment.PaymentDate,
                            PaymentStatus = payment.PaymentStatus,
                        })
                        .ToList();

                    return paymentDetail;
                }
                catch (Exception exception)
                {
                    _logger.LogError(
                        exception,
                        "Error fetching overdue payments: {Message}",
                        exception.Message
                    );
                    throw new CustomException("Error fetching overdue payments.", exception);
                }
            }
        }
    }
}
