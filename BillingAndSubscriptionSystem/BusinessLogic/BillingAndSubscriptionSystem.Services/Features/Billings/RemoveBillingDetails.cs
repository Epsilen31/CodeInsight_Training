using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace BillingAndSubscriptionSystem.Services.Features.Billings
{
    public class RemoveBillingDetails
    {
        public class Command : IRequest<string> { }
    }
}
