using Codeinsight.StreamingManagementSystem.BusinessLogic.Enums;

namespace Codeinsight.StreamingManagementSystem.BusinessLogic.DTOs
{
    public class BillingDto
    {
        public int UserId { get; set; }
        public required string BillingAddress { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
