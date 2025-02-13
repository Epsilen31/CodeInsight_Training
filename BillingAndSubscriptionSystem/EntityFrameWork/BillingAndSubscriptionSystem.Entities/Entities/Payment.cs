using BillingAndSubscriptionSystem.Entities.Enums;

namespace BillingAndSubscriptionSystem.Entities.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public int SubscriptionId { get; set; }
    }
}
