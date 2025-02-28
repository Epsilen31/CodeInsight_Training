namespace BillingAndSubscriptionSystem.Entities.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string? RoleName { get; set; }

        public ICollection<User>? Users { get; set; }

        public static implicit operator Role(string v)
        {
            throw new NotImplementedException();
        }
    }
}
