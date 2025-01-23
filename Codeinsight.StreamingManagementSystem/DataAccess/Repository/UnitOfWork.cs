using Codeinsight.StreamingManagementSystem.DataAccess.Context;
using Codeinsight.StreamingManagementSystem.DataAccess.Contracts;

namespace Codeinsight.StreamingManagementSystem.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseConnection _context;
        private IUserSubscriptionRepository _userSubscriptionRepository;
        private IBillingRepository _billingRepository;
        private IPaymentRepository _paymentRepository;

        private IUserRepository _userRepository;

        public UnitOfWork(DatabaseConnection context)
        {
            _context = context;
        }

        public IUserSubscriptionRepository UserSubscriptionRepository
        {
            get
            {
                if (_userSubscriptionRepository == null)
                {
                    _userSubscriptionRepository = new UserSubscriptionRepository(_context);
                }
                return _userSubscriptionRepository;
            }
        }
        public IBillingRepository BillingRepository
        {
            get
            {
                if (_billingRepository == null)
                {
                    _billingRepository = new BillingRepository(_context);
                }
                return _billingRepository;
            }
        }

        public IPaymentRepository PaymentRepository
        {
            get
            {
                if (_paymentRepository == null)
                {
                    _paymentRepository = new PaymentRepository(_context);
                }
                return _paymentRepository;
            }
        }

        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_context);
                }
                return _userRepository;
            }
        }
    }
}
