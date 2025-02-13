using BillingAndSubscriptionSystem.Entities.Enums;

namespace BillingAndSubscriptionSystem.Services.DTOs
{
    public class BillingDto
    {
        public PaymentMethod PaymentMethod { get; set; }
        public string? BillingAddress { get; set; }
        public int UserId { get; set; }
    }
}
