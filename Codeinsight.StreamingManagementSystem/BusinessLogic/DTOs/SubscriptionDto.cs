using Codeinsight.StreamingManagementSystem.BusinessLogic.Enums;

namespace Codeinsight.StreamingManagementSystem.BusinessLogic.DTOs
{
    public class SubscriptionDto
    {
        public int SubscriptionId { get; set; }
        public int UserId { get; set; }
        public SubscriptionPlanStatus PlanType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public SubscriptionStatus SubscriptionStatus { get; set; }
    }
}
