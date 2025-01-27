using Codeinsight.StreamingManagementSystem.BusinessLogic.Enums;

namespace Codeinsight.StreamingManagementSystem.DataAccess.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public int SubscriptionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentStatus Status { get; set; }
    }
}
