using BillingAndSubscriptionSystem.Entities.Enums;

namespace BillingAndSubscriptionSystem.Entities.Entities
{
    public class Billing
    {
        public int Id { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string? BillingAddress { get; set; }
        public int UserId { get; set; }
    }
}
