using BillingAndSubscriptionSystem.Entities.Enums;

namespace BillingAndSubscriptionSystem.Services.DTOs
{
    public class SubscriptionDto
    {
        public int SubscriptionId { get; set; }
        public int UserId { get; set; }
        public SubscriptionPlanType PlanType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public SubscriptionStatus SubscriptionStatus { get; set; }
    }
}
