using Codeinsight.StreamingManagementSystem.BusinessLogic.Contracts;
using Codeinsight.StreamingManagementSystem.BusinessLogic.DTOs;
using Codeinsight.StreamingManagementSystem.DataAccess.Contracts;
using Codeinsight.StreamingManagementSystem.DataAccess.Entities;

namespace Codeinsight.StreamingManagementSystem.BusinessLogic.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubscriptionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void CreateUserSubscriptonPlan(SubscriptionDto subscription)
        {
            var newSubscription = new Subscription
            {
                UserId = subscription.UserId,
                PlanType = subscription.PlanType,
                StartDate =
                    subscription.StartDate != default ? subscription.StartDate : DateTime.Now,
                EndDate = subscription.EndDate != default ? subscription.EndDate : DateTime.Now,
                SubscriptionStatus =
                    subscription.SubscriptionStatus == 0
                        ? Enums.SubscriptionStatus.Active
                        : Enums.SubscriptionStatus.Inactive,
            };
            _unitOfWork.UserSubscriptionRepository.CreateSubscription(newSubscription);
        }

        public void UpdateUserSubscriptonPlan(SubscriptionDto subscription)
        {
            var updatedSubscription = new Subscription
            {
                UserId = subscription.UserId,
                PlanType = subscription.PlanType,
                StartDate =
                    subscription.StartDate != default ? subscription.StartDate : DateTime.Now,
                EndDate = subscription.EndDate != default ? subscription.EndDate : DateTime.Now,
                SubscriptionStatus =
                    subscription.SubscriptionStatus == 0
                        ? Enums.SubscriptionStatus.Active
                        : Enums.SubscriptionStatus.Inactive,
                Id = subscription.SubscriptionId,
            };
            _unitOfWork.UserSubscriptionRepository.UpdateSubscription(updatedSubscription);
        }

        public ICollection<SubscriptionDto> GetSubscriptionsByUserId(int userId)
        {
            var subscriptions = _unitOfWork.UserSubscriptionRepository.GetSubscriptionsByUserId(
                userId
            );

            var subscriptionDtos = subscriptions
                .Select(sub => new SubscriptionDto
                {
                    SubscriptionId = sub.Id,
                    UserId = sub.UserId,
                    PlanType = sub.PlanType,
                    StartDate = sub.StartDate,
                    EndDate = sub.EndDate,
                    SubscriptionStatus = sub.SubscriptionStatus,
                })
                .ToList();

            return subscriptionDtos;
        }
    }
}
