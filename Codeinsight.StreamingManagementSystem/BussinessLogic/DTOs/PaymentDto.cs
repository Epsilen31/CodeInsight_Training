using Codeinsight.StreamingManagementSystem.BusinessLogic.Enums;

namespace Codeinsight.StreamingManagementSystem.BusinessLogic
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public int SubscriptionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentStatus Status { get; set; }
    }
}
