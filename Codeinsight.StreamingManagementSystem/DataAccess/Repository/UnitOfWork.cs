using Codeinsight.StreamingManagementSystem.DataAccess.Context;
using Codeinsight.StreamingManagementSystem.DataAccess.Contracts;
using Codeinsight.StreamingManagementSystem.Settings;

namespace Codeinsight.StreamingManagementSystem.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseConnection _context;
        private IUserSubscriptionRepository _userSubscriptionRepository;
        private IBillingRepository _billingRepository;
        private IPaymentRepository _paymentRepository;
        private IUserRepository _userRepository;

        public UnitOfWork(AppSetting appSetting)
        {
            _context = DatabaseConnection.GetInstance(appSetting);
            _userSubscriptionRepository = new UserSubscriptionRepository(_context);
            _billingRepository = new BillingRepository(_context);
            _paymentRepository = new PaymentRepository(_context);
            _userRepository = new UserRepository(_context);
        }

        public IUserSubscriptionRepository UserSubscriptionRepository =>
            _userSubscriptionRepository;
        public IBillingRepository BillingRepository => _billingRepository;
        public IPaymentRepository PaymentRepository => _paymentRepository;
        public IUserRepository UserRepository => _userRepository;
    }
}
