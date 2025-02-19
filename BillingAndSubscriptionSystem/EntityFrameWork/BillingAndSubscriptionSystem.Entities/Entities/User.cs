namespace BillingAndSubscriptionSystem.Entities.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public int RoleId { get; set; }

        public virtual Role? Role { get; set; } = null!;
        public virtual ICollection<Subscription>? Subscriptions { get; set; }
        public virtual ICollection<Billing>? Billings { get; set; }
    }
}
