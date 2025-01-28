using Codeinsight.StreamingManagementSystem.BusinessLogic.Enums;
using Codeinsight.StreamingManagementSystem.BusinessLogic.Services;
using Codeinsight.StreamingManagementSystem.Core.Setting;

namespace Codeinsight.StreamingManagementSystem.BusinessLogic.Contracts
{
    public class BillingAndSubscriptionManager : IBillingAndSubscriptionManager
    {
        private readonly AppSetting _appSetting;

        public BillingAndSubscriptionManager(AppSetting appSetting)
        {
            _appSetting = appSetting;
        }

        public void ManageSubscriptionAndBilling()
        {
            var operation = GetBillingAndSubscriptionOperation();

            switch (operation)
            {
                case BillingAndSubscriptionOperations.ManageSubscription:
                    PerformSubscriptionOperation();
                    break;
                case BillingAndSubscriptionOperations.ManageBilling:
                    ManageBilling();
                    break;
                case BillingAndSubscriptionOperations.ManagePayment:
                    ManagePayment();
                    break;
                default:
                    Console.WriteLine("Invalid operation selected.");
                    break;
            }
        }

        private BillingAndSubscriptionOperations GetBillingAndSubscriptionOperation()
        {
            Console.WriteLine("1. Manage Subscription");
            Console.WriteLine("2. Manage Billing");
            Console.WriteLine("3. Manage Payment");

            if (int.TryParse(Console.ReadLine(), out var operation))
            {
                return (BillingAndSubscriptionOperations)operation;
            }

            Console.WriteLine("Invalid operation selected.");
            return BillingAndSubscriptionOperations.None;
        }

        private void PerformSubscriptionOperation()
        {
            Console.WriteLine("1. Add Subscription");
            Console.WriteLine("2. Update Subscription");
            Console.WriteLine("3. Get Subscription By User");

            if (int.TryParse(Console.ReadLine(), out var subscriptionOperation))
            {
                switch ((SubscriptionOperations)subscriptionOperation)
                {
                    case SubscriptionOperations.AddSubscription:
                        PerformUserSubscriptionPlan();
                        break;
                    case SubscriptionOperations.UpdateSubscription:
                        PerformUpdateUserSubscriptionPlan();
                        break;
                    case SubscriptionOperations.GetSubscriptionsByUserId:
                        PerformDisplayUserSubscriptions();
                        break;
                    default:
                        Console.WriteLine("Invalid subscription operation selected.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid subscription operation selected.");
            }
        }

        private void PerformUserSubscriptionPlan()
        {
            SubscriptionManager subscriptionManager = new SubscriptionManager(_appSetting);
            subscriptionManager.ExecuteUserSubscriptionPlan();
        }

        private void PerformUpdateUserSubscriptionPlan()
        {
            SubscriptionManager subscriptionManager = new SubscriptionManager(_appSetting);
            subscriptionManager.ExecuteUpdateUserSubscriptionPlan();
        }

        private void PerformDisplayUserSubscriptions()
        {
            SubscriptionManager subscriptionManager = new SubscriptionManager(_appSetting);
            subscriptionManager.DisplayUserSubscriptions();
        }

        private void ManageBilling()
        {
            Console.WriteLine("1. Update Billing");
            Console.WriteLine("2. Get Billing By User");
            Console.WriteLine("3. Get All Billing By User");

            if (int.TryParse(Console.ReadLine(), out var billingOperation))
            {
                switch ((BillingOperations)billingOperation)
                {
                    case BillingOperations.UpdateBilling:
                        PerformExecuteUpdateBillingDetails();
                        break;
                    case BillingOperations.GetBillingByUser:
                        PerformExecuteViewBillingDetails();
                        break;
                    case BillingOperations.GetAllBillingByUser:
                        PerformExecuteViewAllBillingDetails();
                        break;
                    default:
                        Console.WriteLine("Invalid billing operation selected.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid billing operation selected.");
            }
        }

        private void PerformExecuteUpdateBillingDetails()
        {
            BillingManager billingManager = new BillingManager(_appSetting);
            billingManager.ExecuteUpdateBillingDetails();
        }

        private void PerformExecuteViewBillingDetails()
        {
            BillingManager billingManager = new BillingManager(_appSetting);
            billingManager.ExecuteViewBillingDetails();
        }

        private void PerformExecuteViewAllBillingDetails()
        {
            BillingManager billingManager = new BillingManager(_appSetting);
            billingManager.ExecuteViewAllBillingDetails();
        }

        private void ManagePayment()
        {
            Console.WriteLine("1. Update Payment");
            Console.WriteLine("2. Get Overdue Payment");

            if (int.TryParse(Console.ReadLine(), out var paymentOperation))
            {
                switch ((PaymentOperations)paymentOperation)
                {
                    case PaymentOperations.UpdatePayment:
                        PerformExecuteProcessPayment();
                        break;
                    case PaymentOperations.GetOverduePayment:
                        PerformExecuteGetOverduePayments();
                        break;
                    default:
                        Console.WriteLine("Invalid payment operation selected.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid payment operation selected.");
            }
        }

        private void PerformExecuteProcessPayment()
        {
            PaymentManager paymentManager = new PaymentManager(_appSetting);
            paymentManager.ExecuteProcessPayment();
        }

        private void PerformExecuteGetOverduePayments()
        {
            PaymentManager paymentManager = new PaymentManager(_appSetting);
            paymentManager.ExecuteGetOverduePayments();
        }
    }
}
