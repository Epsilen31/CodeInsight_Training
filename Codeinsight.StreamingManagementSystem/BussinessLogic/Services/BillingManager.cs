using Codeinsight.StreamingManagementSystem.BusinessLogic.Contracts;
using Codeinsight.StreamingManagementSystem.BusinessLogic.DTOs;
using Codeinsight.StreamingManagementSystem.Core.Setting;
using Codeinsight.StreamingManagementSystem.DataAccess.Contracts;
using Codeinsight.StreamingManagementSystem.DataAccess.Repository;

namespace Codeinsight.StreamingManagementSystem.BusinessLogic.Services
{
    public class BillingManager
    {
        private readonly AppSetting _appSetting;

        public BillingManager(AppSetting appSetting)
        {
            _appSetting = appSetting;
        }

        public void ExecuteUpdateBillingDetails()
        {
            int userId = GetUserIdFromUser();
            if (userId == 0)
            {
                return;
            }

            string newBillingAddress = GetNewBillingAddressFromUser();
            if (string.IsNullOrEmpty(newBillingAddress))
            {
                Console.WriteLine("Invalid billing address.");
                return;
            }

            Enums.PaymentMethod newPaymentMethod = GetPaymentMethodFromUser();

            var billingDetail = new BillingDto
            {
                UserId = userId,
                BillingAddress = newBillingAddress,
                PaymentMethod = newPaymentMethod,
            };

            IUnitOfWork unitOfWork = new UnitOfWork(_appSetting);
            IBillingService billingService = new BillingService(unitOfWork);

            billingService.UpdateBillingDetails(billingDetail);
            Console.WriteLine("Billing details updated successfully.");
        }

        private int GetUserIdFromUser()
        {
            Console.WriteLine("Enter User ID:");
            string userIdInput = Console.ReadLine();
            if (int.TryParse(userIdInput, out int userId))
            {
                return userId;
            }
            Console.WriteLine("Invalid user ID. Please enter a valid integer.");
            return 0;
        }

        private string GetNewBillingAddressFromUser()
        {
            Console.WriteLine("Enter New Billing Address:");
            string billingAddress = Console.ReadLine();
            return billingAddress;
        }

        private Enums.PaymentMethod GetPaymentMethodFromUser()
        {
            Console.WriteLine("Select Payment Method:");
            Console.WriteLine("1. Credit Card");
            Console.WriteLine("2. PayPal");

            string paymentChoiceInput = Console.ReadLine();
            if (int.TryParse(paymentChoiceInput, out int paymentChoice))
            {
                if (paymentChoice == 1)
                {
                    return Enums.PaymentMethod.CreditCard;
                }
                else if (paymentChoice == 2)
                {
                    return Enums.PaymentMethod.PayPal;
                }
            }

            return Enums.PaymentMethod.CreditCard;
        }

        public void ExecuteViewBillingDetails()
        {
            int userId = GetUserIdFromUser();
            if (userId == 0)
            {
                return;
            }
            IUnitOfWork unitOfWork = new UnitOfWork(_appSetting);
            IBillingService billingService = new BillingService(unitOfWork);

            try
            {
                var billingDetails = billingService.GetBillingWithUserDetails(userId);
                foreach (var billing in billingDetails)
                {
                    Console.WriteLine(
                        $"UserId: {billing.UserId}, BillingAddress: {billing.BillingAddress}, PaymentMethod: {billing.PaymentMethod}"
                    );
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(
                    "An error occurred while retrieving billing details: " + exception.Message
                );
            }
        }

        public void ExecuteViewAllBillingDetails()
        {
            IUnitOfWork unitOfWork = new UnitOfWork(_appSetting);
            IBillingService billingService = new BillingService(unitOfWork);
            try
            {
                var allBillingDetails = billingService.GetAllUsersWithBillingDetails();
                foreach (var billing in allBillingDetails)
                {
                    Console.WriteLine(
                        $"UserId: {billing.UserId}, BillingAddress: {billing.BillingAddress}, PaymentMethod: {billing.PaymentMethod}"
                    );
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(
                    "An error occurred while retrieving billing details: " + exception.Message
                );
            }
        }
    }
}
