using Codeinsight.StreamingManagementSystem.BusinessLogic.DTOs;

namespace Codeinsight.StreamingManagementSystem.BusinessLogic.Contracts
{
    public interface IBillingService
    {
        void UpdateBillingDetails(BillingDto billing);
        public ICollection<BillingDto> GetBillingWithUserDetails(int userId);
        public ICollection<BillingDto> GetAllUsersWithBillingDetails();
    }
}
