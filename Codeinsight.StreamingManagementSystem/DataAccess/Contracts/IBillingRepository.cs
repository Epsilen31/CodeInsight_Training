using Codeinsight.StreamingManagementSystem.DataAccess.Entities;

namespace Codeinsight.StreamingManagementSystem.DataAccess.Contracts
{
    public interface IBillingRepository
    {
        ICollection<Billing> GetBillingWithUserDetails(int userId);
        void UpdateBillingDetails(Billing billingDetails);
        ICollection<Billing> GetAllUsersWithBillingDetails();
    }
}
