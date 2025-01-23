namespace Codeinsight.StreamingManagementSystem.DataAccess.Entities
{
    public class Subscription
    {
        public int SubscriptionId { get; set; }
        public int UserId { get; set; }
        public required string Plan { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
