using BillingAndSubscriptionSystem.Entities.Entities;

namespace BillingAndSubscriptionSystem.DataAccess.Contracts
{
    public interface IBillingRepository
    {
        Task UpdateBillingDetails(Billing billingDetails, CancellationToken cancellationToken);
        Task<ICollection<Billing>> GetAllUsersWithBillingDetails(
            CancellationToken cancellationToken
        );
    }
}
