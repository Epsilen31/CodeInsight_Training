using Codeinsight.StreamingManagementSystem.BusinessLogic.DTOs;

namespace Codeinsight.StreamingManagementSystem.BusinessLogic.Contracts
{
    public interface ISubscriptionService
    {
        void CreateUserSubscriptonPlan(SubscriptionDto subscription);
        void UpdateUserSubscriptonPlan(SubscriptionDto subscription);
        ICollection<SubscriptionDto> GetSubscriptionsByUserId(int userId);
    }
}
