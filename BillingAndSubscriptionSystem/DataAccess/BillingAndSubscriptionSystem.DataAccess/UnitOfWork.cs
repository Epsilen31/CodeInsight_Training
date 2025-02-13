using BillingAndSubscriptionSystem.Context;
using BillingAndSubscriptionSystem.DataAccess.Contracts;
using BillingAndSubscriptionSystem.DataAccess.Repositories;

namespace BillingAndSubscriptionSystem.DataAccess
{
    public class UnitOfWork : IUnitOfWork
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
            _billingRepository = new BillingRepository(_context);
            _userRepository = new UserRepository(_context);
            _paymentRepository = new PaymentRepository(_context);
            _userSubscriptionRepository = new UserSubscriptionRepository(_context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
