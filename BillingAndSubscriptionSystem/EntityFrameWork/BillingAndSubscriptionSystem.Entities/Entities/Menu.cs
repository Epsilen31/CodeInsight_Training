namespace BillingAndSubscriptionSystem.Entities.Entities
{
    public class Menu
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public string Icon { get; set; } = string.Empty;
        public string Role { get; set; } = "Admin";
        public ICollection<SubMenu> SubMenus { get; set; } = [];
    }
}
