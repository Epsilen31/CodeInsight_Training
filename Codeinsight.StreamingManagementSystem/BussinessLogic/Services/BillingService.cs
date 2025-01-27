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
            var billingEntities = _unitOfWork.BillingRepository.GetBillingWithUserDetails(userId);
            var billingDtos = billingEntities
                .Select(bills => new BillingDto
                {
                    UserId = bills.UserId,
                    BillingAddress = bills.BillingAddress,
                    PaymentMethod = bills.PaymentMethod,
                })
                .ToList();

            return billingDtos;
        }

        public ICollection<BillingDto> GetAllUsersWithBillingDetails()
        {
            var billingEntities = _unitOfWork.BillingRepository.GetAllUsersWithBillingDetails();
            var billingDtos = billingEntities
                .Select(bills => new BillingDto
                {
                    UserId = bills.UserId,
                    BillingAddress = bills.BillingAddress,
                    PaymentMethod = bills.PaymentMethod,
                })
                .ToList();

            return billingDtos;
        }
    }
}
