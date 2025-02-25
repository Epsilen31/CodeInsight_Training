using BillingAndSubscriptionSystem.Entities.Entities;
using BillingAndSubscriptionSystem.Services.DTOs;
using Mapster;

namespace BillingAndSubscriptionSystem.Services.Mapster
{
    public class SubscriptionMapsterConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // Mapping SubscriptionDto → Subscription
            config
                .ForType<SubscriptionDto, Subscription>()
                .AfterMapping(
                    (source, target) =>
                    {
                        target.Id = source.SubscriptionId;
                    }
                );

            // Mapping Subscription → SubscriptionDto
            config
                .ForType<Subscription, SubscriptionDto>()
                .AfterMapping(
                    (source, target) =>
                    {
                        target.SubscriptionId = source.Id;
                    }
                );
        }
    }
}
