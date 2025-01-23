namespace Codeinsight.StreamingManagementSystem.BusinessLogic.DTOs
{
    public class SubscriptionDto
    {
        public int UserId { get; set; }
        public PlanType PlanType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public required string SubscriptionStatus { get; set; }
    }
}
