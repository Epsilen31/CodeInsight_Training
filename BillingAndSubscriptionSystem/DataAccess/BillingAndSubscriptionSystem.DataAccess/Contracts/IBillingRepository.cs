using BillingAndSubscriptionSystem.Entities.Entities;

namespace BillingAndSubscriptionSystem.DataAccess.Contracts
{
    public interface IBillingRepository
    {
        Task UpdateBillingAsync(Billing billingDetails, CancellationToken cancellationToken);
        Task<ICollection<Billing>> GetAllUsersWithBillingDetails(
            int userId,
            CancellationToken cancellationToken
        );

        Task RemoveBillingDetailsAsync(int userId, CancellationToken cancellationToken);
    }
}
