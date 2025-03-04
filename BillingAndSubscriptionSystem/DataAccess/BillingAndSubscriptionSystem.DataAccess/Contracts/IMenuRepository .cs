using BillingAndSubscriptionSystem.Entities.Entities;

namespace BillingAndSubscriptionSystem.DataAccess.Contracts
{
    public interface IMenuRepository
    {
        Task<ICollection<Menu>> GetSidebarMenuAsync(
            string role,
            CancellationToken cancellationToken
        );
    }
}
