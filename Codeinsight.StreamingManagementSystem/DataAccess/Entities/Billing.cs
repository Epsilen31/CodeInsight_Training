public class Billing
{
    public int BillingId { get; set; }
    public int UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime BillingDate { get; set; }
}
