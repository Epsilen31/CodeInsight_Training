using BillingAndSubscriptionSystem.Context;
using BillingAndSubscriptionSystem.DataAccess.Contracts;
using BillingAndSubscriptionSystem.DataAccess.Repositories;

namespace BillingAndSubscriptionSystem.DataAccess
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly BillingDbContext _context;

        private IUserRepository? _userRepository;
        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);

        private IPaymentRepository? _paymentRepository;
        public IPaymentRepository PaymentRepository =>
            _paymentRepository ??= new PaymentRepository(_context);

        private IUserSubscriptionRepository? _userSubscriptionRepository;
        public IUserSubscriptionRepository UserSubscriptionRepository =>
            _userSubscriptionRepository ??= new UserSubscriptionRepository(_context);

        private IBillingRepository? _billingRepository;
        public IBillingRepository BillingRepository =>
            _billingRepository ??= new BillingRepository(_context);

        public UnitOfWork(BillingDbContext context)
        {
            _context = context;
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
