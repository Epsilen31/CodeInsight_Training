using Codeinsight.StreamingManagementSystem.BusinessLogic.Contracts;
using Codeinsight.StreamingManagementSystem.BusinessLogic.DTOs;
using Codeinsight.StreamingManagementSystem.DataAccess.Contracts;
using Codeinsight.StreamingManagementSystem.DataAccess.Entities;

namespace Codeinsight.StreamingManagementSystem.BusinessLogic.Services
{
    public class BillingService : IBillingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BillingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void UpdateBillingDetails(BillingDto billing)
        {
            if (billing == null)
            {
                throw new ArgumentNullException("Billing details cannot be null.");
            }

            if (
                string.IsNullOrEmpty(billing.BillingAddress)
                || billing.PaymentMethod == Enums.PaymentMethod.CreditCard
                || billing.PaymentMethod == Enums.PaymentMethod.PayPal
            )
            {
                throw new ArgumentException("Billing address and payment method cannot be empty.");
            }

            var billingEntity = new Billing
            {
                UserId = billing.UserId,
                BillingAddress = billing.BillingAddress,
                PaymentMethod = billing.PaymentMethod,
            };

            _unitOfWork.BillingRepository.UpdateBillingDetails(billingEntity);
        }

        public ICollection<BillingDto> GetBillingWithUserDetails(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be a positive integer.");
            }
            var billingEntities = _unitOfWork.BillingRepository.GetBillingWithUserDetails(userId);
            var billings = billingEntities
                .Select(bills => new BillingDto
                {
                    UserId = bills.UserId,
                    BillingAddress = bills.BillingAddress,
                    PaymentMethod = bills.PaymentMethod,
                })
                .ToList();

            return billings;
        }

        public ICollection<BillingDto> GetAllUsersWithBillingDetails()
        {
            var billingEntities = _unitOfWork.BillingRepository.GetAllUsersWithBillingDetails();

            if (billingEntities.Count == 0)
            {
                throw new InvalidOperationException("No billing details found.");
            }

            var billings = billingEntities
                .Select(bills => new BillingDto
                {
                    UserId = bills.UserId,
                    BillingAddress = bills.BillingAddress,
                    PaymentMethod = bills.PaymentMethod,
                })
                .ToList();

            return billings;
        }
    }
}
