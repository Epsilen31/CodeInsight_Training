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

        public void CreateUserSubscriptionPlan(SubscriptionDto subscription)
        {
            if (subscription.UserId <= 0)
            {
                throw new ArgumentException("Invalid subscription parameters.");
            }
            var existingSubscription =
                _unitOfWork.UserSubscriptionRepository.GetSubscriptionsByUserId(
                    subscription.UserId
                );

            if (existingSubscription != null)
            {
                throw new ArgumentException("Subscription already exists");
            }

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

        public void UpdateUserSubscriptionPlan(SubscriptionDto subscription)
        {
            if (subscription.UserId <= 0)
            {
                throw new ArgumentException("Invalid subscription parameters.");
            }
            var existingSubscription =
                _unitOfWork.UserSubscriptionRepository.GetSubscriptionsByUserId(
                    subscription.UserId
                );

            if (existingSubscription == null)
            {
                throw new ArgumentException("Subscription not found");
            }

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
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid user ID.");
            }

            var subscriptions = _unitOfWork.UserSubscriptionRepository.GetSubscriptionsByUserId(
                userId
            );

            if (subscriptions == null)
            {
                return new List<SubscriptionDto>();
            }

            var subscription = subscriptions
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

            return subscription;
        }
    }
}
