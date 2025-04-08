using MediatR;

namespace BillingAndSubscriptionSystem.Services.Features.Billings
{
    public class RemoveBillingDetails
    {
        public class Command : IRequest<string> { }
    }
}
