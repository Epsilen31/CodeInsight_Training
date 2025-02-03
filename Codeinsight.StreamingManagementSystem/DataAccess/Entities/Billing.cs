using Codeinsight.StreamingManagementSystem.BusinessLogic.Enums;

namespace Codeinsight.StreamingManagementSystem.DataAccess.Entities
{
    public class Billing
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string BillingAddress { get; set; }
    }
}
