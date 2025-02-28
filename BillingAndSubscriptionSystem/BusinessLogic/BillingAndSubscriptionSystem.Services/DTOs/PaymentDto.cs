using BillingAndSubscriptionSystem.Entities.Enums;

namespace BillingAndSubscriptionSystem.Services.DTOs
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public int SubscriptionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }
}
