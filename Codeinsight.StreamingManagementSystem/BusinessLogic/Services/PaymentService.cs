using Codeinsight.StreamingManagementSystem.BusinessLogic.Contracts;
using Codeinsight.StreamingManagementSystem.BusinessLogic.DTOs;
using Codeinsight.StreamingManagementSystem.DataAccess.Contracts;
using Codeinsight.StreamingManagementSystem.DataAccess.Entities;
using Dapper;

namespace Codeinsight.StreamingManagementSystem.BusinessLogic.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void ProcessPayment(PaymentDto paymentDetails)
        {
            if (paymentDetails == null)
            {
                throw new ArgumentNullException(" paymentDetails cannot be null");
            }
            Payment createPaymentDetails = new Payment()
            {
                SubscriptionId = paymentDetails.SubscriptionId,
                Amount = paymentDetails.Amount,
                PaymentDate = paymentDetails.PaymentDate,
                Status =
                    paymentDetails.Status == 0
                        ? Enums.PaymentStatus.Paid
                        : Enums.PaymentStatus.Overdue,
            };
            _unitOfWork.PaymentRepository.ProcessPayment(createPaymentDetails);
        }

        public ICollection<PaymentDto> GetOverduePayments()
        {
            var overduePayments = _unitOfWork.PaymentRepository.GetOverduePayments();
            if (overduePayments == null)
            {
                return new List<PaymentDto>();
            }
            return overduePayments
                .Select(payment => new PaymentDto
                {
                    Id = payment.Id,
                    SubscriptionId = payment.SubscriptionId,
                    Amount = payment.Amount,
                    PaymentDate = payment.PaymentDate,
                    Status = payment.Status,
                })
                .AsList();
        }
    }
}
