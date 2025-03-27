namespace BillingAndSubscriptionSystem.Services.DTOs
{
    public class SubMenuDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Path { get; set; }
        public bool IsActive { get; set; }
        public string? Icon { get; set; }
    }
}
