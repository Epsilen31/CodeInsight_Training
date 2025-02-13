using BillingAndSubscriptionSystem.Entities.Enums;

namespace BillingAndSubscriptionSystem.Entities.Entities
{
    public class Subscription
    {
        public int Id { get; set; }
        public SubscriptionPlanType PlanType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public SubscriptionStatus SubscriptionStatus { get; set; }
        public int UserId { get; set; }

        public virtual ICollection<Payment>? Payments { get; set; }
    }
}
