using BillingAndSubscriptionSystem.Core.Exceptions;
using BillingAndSubscriptionSystem.DataAccess;
using BillingAndSubscriptionSystem.Services.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BillingAndSubscriptionSystem.Services.Features.Billing
{
    public class UpdateBillingDetails
    {
        public class Command : IRequest<Unit>
        {
            public BillingDto Billing { get; set; }

            public Command(BillingDto billing)
            {
                Billing = billing;
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly UnitOfWork _unitOfWork;
            private readonly ILogger<UpdateBillingDetails> _logger;

            public Handler(UnitOfWork unitOfWork, ILogger<UpdateBillingDetails> logger)
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    ValidateRequest(request.Billing);
                    var billingEntity = MapBilling(request.Billing);
                    await _unitOfWork.BillingRepository.UpdateBillingAsync(
                        billingEntity,
                        cancellationToken
                    );
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    return Unit.Value;
                }
                catch (Exception exception)
                {
                    _logger.LogError(
                        exception,
                        "Error updating billing details Exception: {Exception}",
                        exception.Message
                    );
                    throw new CustomException("Error updating billing details", exception);
                }
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
