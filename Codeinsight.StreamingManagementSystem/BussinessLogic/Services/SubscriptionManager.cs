using Codeinsight.StreamingManagementSystem.BusinessLogic.Contracts;
using Codeinsight.StreamingManagementSystem.BusinessLogic.DTOs;
using Codeinsight.StreamingManagementSystem.Core.Setting;
using Codeinsight.StreamingManagementSystem.DataAccess.Contracts;
using Codeinsight.StreamingManagementSystem.DataAccess.Repository;

namespace Codeinsight.StreamingManagementSystem.BusinessLogic.Services
{
    public class SubscriptionManager
    {
        private readonly AppSetting _appSetting;

        public SubscriptionManager(AppSetting appSetting)
        {
            _appSetting = appSetting;
        }

        public void ExecuteUserSubscriptionPlan()
        {
            try
            {
                Console.WriteLine("Enter User ID:");
                int userId = int.Parse(Console.ReadLine());

                Enums.PlanType planType = GetPlanTypeFromUser();

                Console.WriteLine("Enter Subscription Duration in Months:");
                int durationInMonths = int.Parse(Console.ReadLine());

                var subscription = new SubscriptionDto
                {
                    UserId = userId,
                    PlanType = (Enums.SubscriptionPlanStatus)planType,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(durationInMonths),
                    SubscriptionStatus = Enums.SubscriptionStatus.Active,
                };

                IUnitOfWork unitOfWork = new UnitOfWork(_appSetting);
                ISubscriptionService subscriptionService = new SubscriptionService(unitOfWork);

                subscriptionService.CreateUserSubscriptionPlan(subscription);

                Console.WriteLine("Subscription created successfully.");
            }
            catch (Exception exception)
            {
                Console.WriteLine(
                    "An error occurred while creating subscription plan: " + exception.Message
                );
            }
        }

        public void ExecuteUpdateUserSubscriptionPlan()
        {
            try
            {
                Console.WriteLine("Enter User ID:");
                int userId = int.Parse(Console.ReadLine());

                Enums.PlanType planType = GetPlanTypeFromUser();

                Console.WriteLine("Enter New Subscription Duration in Months:");
                int durationInMonths = int.Parse(Console.ReadLine());

                var subscription = new SubscriptionDto
                {
                    UserId = userId,
                    PlanType = (Enums.SubscriptionPlanStatus)planType,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(durationInMonths),
                    SubscriptionStatus = Enums.SubscriptionStatus.Active,
                };

                IUnitOfWork unitOfWork = new UnitOfWork(_appSetting);
                ISubscriptionService subscriptionService = new SubscriptionService(unitOfWork);

                subscriptionService.UpdateUserSubscriptionPlan(subscription);

                Console.WriteLine("Subscription updated successfully.");
            }
            catch (Exception exception)
            {
                Console.WriteLine(
                    "An error occurred while updating subscription details: " + exception.Message
                );
            }
        }

        public void DisplayUserSubscriptions()
        {
            try
            {
                IUnitOfWork unitOfWork = new UnitOfWork(_appSetting);
                ISubscriptionService subscriptionService = new SubscriptionService(unitOfWork);

                Console.WriteLine("Enter User ID:");
                int userId = int.Parse(Console.ReadLine());
                if (userId <= 0)
                {
                    Console.WriteLine("Invalid user ID.");
                    return;
                }

                var subscriptions = subscriptionService.GetSubscriptionsByUserId(userId);

                if (subscriptions != null)
                {
                    Console.WriteLine($"Subscriptions for User ID: {userId}");
                    foreach (var subscription in subscriptions)
                    {
                        Console.WriteLine($"Subscription ID: {subscription.SubscriptionId}");
                        Console.WriteLine($"Plan Type: {subscription.PlanType}");
                        Console.WriteLine(
                            $"Start Date: {subscription.StartDate.ToShortDateString()}"
                        );
                        Console.WriteLine($"End Date: {subscription.EndDate.ToShortDateString()}");
                        Console.WriteLine($"Status: {subscription.SubscriptionStatus}");
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("No subscriptions found for this user.");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(
                    $"An error occurred while fetching subscriptions: {exception.Message}"
                );
            }
        }

        private Enums.PlanType GetPlanTypeFromUser()
        {
            Enums.PlanType planType;
            Console.WriteLine("Select Plan Type:");
            Console.WriteLine("1. Basic");
            Console.WriteLine("2. Premium");
            Console.WriteLine("3. VIP");

            try
            {
                int planChoice = int.Parse(Console.ReadLine());

                switch (planChoice)
                {
                    case (int)Enums.PlanType.Basic:
                        planType = Enums.PlanType.Basic;
                        break;
                    case (int)Enums.PlanType.Premium:
                        planType = Enums.PlanType.Premium;
                        break;
                    case (int)Enums.PlanType.VIP:
                        planType = Enums.PlanType.VIP;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Defaulting to Basic plan.");
                        planType = Enums.PlanType.Basic;
                        break;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(
                    $"An error occurred while getting plan type: {exception.Message}"
                );
                throw;
            }

            return planType;
        }
    }
}
