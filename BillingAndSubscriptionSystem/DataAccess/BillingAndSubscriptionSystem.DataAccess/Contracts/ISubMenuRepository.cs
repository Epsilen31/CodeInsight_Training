using BillingAndSubscriptionSystem.Entities.Entities;

namespace BillingAndSubscriptionSystem.DataAccess.Contracts
{
    public interface ISubMenuRepository
    {
        Task<ICollection<SubMenu>> GetAllActiveSubMenusAsync(CancellationToken cancellationToken);
    }
}
