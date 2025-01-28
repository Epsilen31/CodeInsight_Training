using Codeinsight.StreamingManagementSystem.BusinessLogic.DTOs;

namespace Codeinsight.StreamingManagementSystem.BusinessLogic.Contracts
{
    public interface IBillingService
    {
        void UpdateBillingDetails(BillingDto billing);
        ICollection<BillingDto> GetBillingWithUserDetails(int userId);
        ICollection<BillingDto> GetAllUsersWithBillingDetails();
    }
}
