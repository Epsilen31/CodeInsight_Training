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

        public async Task<ICollection<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _context
                .Users.Where(x => x.Id == user.Id)
                .ExecuteUpdateAsync(ex =>
                    ex.SetProperty(x => x.Name, user.Name)
                        .SetProperty(x => x.Email, user.Email)
                        .SetProperty(x => x.Phone, user.Phone)
                );
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync([id]);
            if (user != null)
            {
                _context.Users.Remove(user);
                return true;
            }
            return false;
        }
    }
}
