namespace BillingAndSubscriptionSystem.Entities.Entities
{
    public class SubMenu
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public int MenuId { get; set; }

        public string Role { get; set; } = "Admin";

        public Menu? Menu { get; set; }
    }
}
