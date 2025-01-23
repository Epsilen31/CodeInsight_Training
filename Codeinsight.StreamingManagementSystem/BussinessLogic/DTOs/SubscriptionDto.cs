namespace Codeinsight.StreamingManagementSystem.BusinessLogic.DTOs
{
    public class SubscriptionDto
    {
        public int UserId { get; set; }
        public string PlanType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}