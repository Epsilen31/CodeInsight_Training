using BillingAndSubscriptionSystem.Entities.Entities;

namespace BillingAndSubscriptionSystem.DataAccess.Contracts
{
    public interface IBillingRepository
    {
        Task UpdateBillingDetails(Billing billingDetails);
        Task<ICollection<Billing>> GetAllUsersWithBillingDetails();
    }
}
