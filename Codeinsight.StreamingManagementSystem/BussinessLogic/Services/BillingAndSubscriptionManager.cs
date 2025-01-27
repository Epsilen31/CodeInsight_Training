using Codeinsight.StreamingManagementSystem.BusinessLogic.Enums;
using Codeinsight.StreamingManagementSystem.BusinessLogic.Services;
using Codeinsight.StreamingManagementSystem.DataAccess.Contracts;

namespace Codeinsight.StreamingManagementSystem.BusinessLogic.Contracts
{
    public class BillingAndSubscriptionManager : IBillingAndSubscriptionManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SubscriptionManager _subscriptionManager;
        private readonly BillingManager _billingManager;
        private readonly PaymentManager _paymentManager;

        public BillingAndSubscriptionManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _subscriptionManager = new SubscriptionManager(_unitOfWork);
            _billingManager = new BillingManager(_unitOfWork);
            _paymentManager = new PaymentManager(_unitOfWork);
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

            if (int.TryParse(Console.ReadLine(), out var subscriptionOperation))
            {
                switch ((SubscriptionOperations)subscriptionOperation)
                {
                    case SubscriptionOperations.AddSubscription:
                        _subscriptionManager.ExecuteUserSubscriptionPlan();
                        break;
                    case SubscriptionOperations.UpdateSubscription:
                        _subscriptionManager.ExecuteUpdateUserSubscriptionPlan();
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

        private void ManageBilling()
        {
            _billingManager.ExecuteUpdateBillingDetails();
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
                        _paymentManager.ExecuteProcessPayment();
                        break;
                    case PaymentOperations.GetOverduePayment:
                        _paymentManager.ExecuteGetOverduePayments();
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
    }
}
