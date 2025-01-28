using Codeinsight.StreamingManagementSystem.BusinessLogic.Contracts;
using Codeinsight.StreamingManagementSystem.BusinessLogic.DTOs;
using Codeinsight.StreamingManagementSystem.Core.Setting;
using Codeinsight.StreamingManagementSystem.DataAccess.Contracts;
using Codeinsight.StreamingManagementSystem.DataAccess.Repository;

namespace Codeinsight.StreamingManagementSystem.BusinessLogic.Services
{
    public class PaymentManager
    {
        private readonly AppSetting _appSetting;

        public PaymentManager(AppSetting appSetting)
        {
            _appSetting = appSetting;
        }

        public void ExecuteProcessPayment()
        {
            try
            {
                Console.WriteLine("Enter Subscription ID:");
                if (!int.TryParse(Console.ReadLine(), out var subscriptionId))
                {
                    Console.WriteLine("Invalid input for Subscription ID.");
                    return;
                }

                Console.WriteLine("Enter The Amount:");
                if (!decimal.TryParse(Console.ReadLine(), out var amount))
                {
                    Console.WriteLine("Invalid input for Amount.");
                    return;
                }

                Console.WriteLine("Enter The Payment Date (yyyy-MM-dd):");
                if (!DateTime.TryParse(Console.ReadLine(), out var paymentDate))
                {
                    Console.WriteLine("Invalid input for Payment Date.");
                    return;
                }

                Enums.PaymentStatus paymentStatus = GetPaymentStatus();

                var paymentDetail = new PaymentDto
                {
                    SubscriptionId = subscriptionId,
                    Amount = amount,
                    PaymentDate = paymentDate,
                    Status = paymentStatus,
                };

                IUnitOfWork unitOfWork = new UnitOfWork(_appSetting);
                IPaymentService paymentService = new PaymentService(unitOfWork);

                paymentService.ProcessPayment(paymentDetail);

                Console.WriteLine("Payment processed successfully.");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"An error occurred: {exception.Message}");
            }
        }

        public ICollection<PaymentDto> ExecuteGetOverduePayments()
        {
            try
            {
                IUnitOfWork unitOfWork = new UnitOfWork(_appSetting);
                IPaymentService paymentService = new PaymentService(unitOfWork);

                var overduePaymentsDetails = paymentService.GetOverduePayments();

                Console.WriteLine("Overdue payments:");
                foreach (var payment in overduePaymentsDetails)
                {
                    Console.WriteLine(
                        $"Subscription ID: {payment.SubscriptionId}, Amount: {payment.Amount}, Payment Date: {payment.PaymentDate}, Status: {payment.Status}"
                    );
                }

                return overduePaymentsDetails;
            }
            catch (Exception exception)
            {
                Console.WriteLine(
                    $"An error occurred while fetching overdue payments: {exception.Message}"
                );
                return new List<PaymentDto>();
            }
        }

        private Enums.PaymentStatus GetPaymentStatus()
        {
            Enums.PaymentStatus paymentStatus;

            Console.WriteLine("Select Payment Status:");
            Console.WriteLine("1. Paid");
            Console.WriteLine("2. Overdue");

            if (int.TryParse(Console.ReadLine(), out var status))
            {
                switch (status)
                {
                    case (int)Enums.PaymentStatus.Paid:
                        paymentStatus = Enums.PaymentStatus.Paid;
                        break;
                    case (int)Enums.PaymentStatus.Overdue:
                        paymentStatus = Enums.PaymentStatus.Overdue;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Defaulting to Overdue.");
                        paymentStatus = Enums.PaymentStatus.Overdue;
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Defaulting to Overdue.");
                paymentStatus = Enums.PaymentStatus.Overdue;
            }

            return paymentStatus;
        }
    }
}
