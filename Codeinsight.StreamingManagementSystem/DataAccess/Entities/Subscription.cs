using Codeinsight.StreamingManagementSystem.BusinessLogic.Enums;

namespace Codeinsight.StreamingManagementSystem.DataAccess.Entities
{
    public class Subscription
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public SubscriptionPlanStatus PlanType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public SubscriptionStatus SubscriptionStatus { get; set; }
    }
}
