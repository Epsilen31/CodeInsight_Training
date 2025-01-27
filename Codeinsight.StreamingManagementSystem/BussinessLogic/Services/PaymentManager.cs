using Codeinsight.StreamingManagementSystem.BusinessLogic.Contracts;
using Codeinsight.StreamingManagementSystem.BusinessLogic.DTOs;
using Codeinsight.StreamingManagementSystem.DataAccess.Contracts;

namespace Codeinsight.StreamingManagementSystem.BusinessLogic.Services
{
    public class PaymentManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

                var paymentDetails = new PaymentDto
                {
                    SubscriptionId = subscriptionId,
                    Amount = amount,
                    PaymentDate = paymentDate,
                    Status = paymentStatus,
                };

                IPaymentService paymentService = new PaymentService(_unitOfWork);
                paymentService.ProcessPayment(paymentDetails);

                Console.WriteLine("Payment processed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public ICollection<PaymentDto> ExecuteGetOverduePayments()
        {
            try
            {
                IPaymentService paymentService = new PaymentService(_unitOfWork);
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
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"An error occurred while fetching overdue payments: {ex.Message}"
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
