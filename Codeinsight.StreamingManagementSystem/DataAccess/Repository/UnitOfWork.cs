using Codeinsight.StreamingManagementSystem.Core.Setting;
using Codeinsight.StreamingManagementSystem.DataAccess.Context;
using Codeinsight.StreamingManagementSystem.DataAccess.Contracts;

namespace Codeinsight.StreamingManagementSystem.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseConnection _context;

        private IUserSubscriptionRepository _userSubscriptionRepository;
        public IUserSubscriptionRepository UserSubscriptionRepository =>
            _userSubscriptionRepository ??= new UserSubscriptionRepository(_context);

        private IBillingRepository _billingRepository;
        public IBillingRepository BillingRepository =>
            _billingRepository ??= new BillingRepository(_context);

        private IPaymentRepository _paymentRepository;
        public IPaymentRepository PaymentRepository =>
            _paymentRepository ??= new PaymentRepository(_context);

        private IUserRepository _userRepository;
        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);

        public UnitOfWork(AppSetting appSetting)
        {
            _context = DatabaseConnection.GetInstance(appSetting);
            _billingRepository = new BillingRepository(_context);
            _paymentRepository = new PaymentRepository(_context);
            _userRepository = new UserRepository(_context);
        }
    }
}
