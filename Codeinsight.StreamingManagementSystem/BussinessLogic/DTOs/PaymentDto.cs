namespace Codeinsight.StreamingManagementSystem.BusinessLogic
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public int SubscriptionId { get; set; }
        public float Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentStatus Status { get; set; }
    }
}
