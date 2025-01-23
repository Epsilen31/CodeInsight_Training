namespace Codeinsight.StreamingManagementSystem.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseConnection _context;
        public UnitOfWork(DbContext context)
        {
            _context = context;
        }
        public IRepository<Subscription> ManageSubscriptions => new Repository<Subscription>(_context);
        public IRepository<Payment> ProcessPayments => new Repository<Payment>(_context);
        public IRepository<Billing> ManageBilling => new Repository<Billing>(_context);
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
