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
                            .SetProperty(x => x.Phone, user.Phone),
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
    }
}
