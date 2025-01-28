using Codeinsight.StreamingManagementSystem.BusinessLogic.DTOs;

namespace Codeinsight.StreamingManagementSystem.BusinessLogic.Contracts
{
    public interface ISubscriptionService
    {
        void CreateUserSubscriptionPlan(SubscriptionDto subscription);
        void UpdateUserSubscriptionPlan(SubscriptionDto subscription);
        ICollection<SubscriptionDto> GetSubscriptionsByUserId(int userId);
    }
}
