using Codeinsight.StreamingManagementSystem.BusinessLogic.Contracts;
using Codeinsight.StreamingManagementSystem.BusinessLogic.DTOs;
using Codeinsight.StreamingManagementSystem.DataAccess.Contracts;

namespace Codeinsight.StreamingManagementSystem.BusinessLogic.Services
{
    public class BillingManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public BillingManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

            IBillingService billingService = new BillingService(_unitOfWork);
            try
            {
                billingService.UpdateBillingDetails(billingDetail);
                Console.WriteLine("Billing details updated successfully.");
            }
            catch (Exception exception)
            {
                Console.WriteLine(
                    "An error occurred while updating billing details: " + exception.Message
                );
            }
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

            IBillingService billingService = new BillingService(_unitOfWork);
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
            IBillingService billingService = new BillingService(_unitOfWork);
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
