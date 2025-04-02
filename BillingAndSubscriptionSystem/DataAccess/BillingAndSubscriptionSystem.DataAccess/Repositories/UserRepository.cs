using BillingAndSubscriptionSystem.Context;
using BillingAndSubscriptionSystem.DataAccess.Contracts;
using BillingAndSubscriptionSystem.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace BillingAndSubscriptionSystem.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BillingDbContext _context;

        public UserRepository(BillingDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<User>> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            return await _context.Users.Include(u => u.Role).ToListAsync(cancellationToken);
        }

        public async Task<User?> GetUserByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Users.FirstOrDefaultAsync(
                user => user.Id == id,
                cancellationToken
            );
        }

        public async Task AddUserAsync(User user, CancellationToken cancellationToken)
        {
            await _context.Users.AddAsync(user, cancellationToken);
        }

        public async Task UpdateUserAsync(User user, CancellationToken cancellationToken)
        {
            await _context
                .Users.Where(x => x.Id == user.Id)
                .ExecuteUpdateAsync(
                    ex =>
                        ex.SetProperty(x => x.Name, user.Name)
                            .SetProperty(x => x.Email, user.Email)
                            .SetProperty(x => x.Phone, user.Phone)
                            .SetProperty(x => x.Password, user.Password),
                    cancellationToken
                );
        }

        public async Task<bool> DeleteUserAsync(int id, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync([id], cancellationToken);
            if (user != null)
            {
                _context.Users.Remove(user);
                return true;
            }
            return false;
        }

        public async Task<bool> ExistsAsync(int userId)
        {
            return await _context.Users.AnyAsync(user => user.Id == userId);
        }

        public async Task<int> GetInactiveUsersCountAsync(CancellationToken cancellationToken)
        {
            return await _context
                .Users.Where(user =>
                    user.Subscriptions == null
                    || !user.Subscriptions.Any(subscription =>
                        subscription.SubscriptionStatus == Entities.Enums.SubscriptionStatus.Active
                    )
                )
                .CountAsync(cancellationToken);
        }

        public async Task<int> GetTotalUsersCountAsync(CancellationToken cancellationToken)
        {
            return await _context.Users.CountAsync(cancellationToken);
        }
    }
}
