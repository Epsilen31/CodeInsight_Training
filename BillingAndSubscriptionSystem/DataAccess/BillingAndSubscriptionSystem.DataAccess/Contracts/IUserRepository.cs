using BillingAndSubscriptionSystem.Entities.Entities;

namespace BillingAndSubscriptionSystem.DataAccess.Contracts
{
    public interface IUserRepository
    {
        Task<ICollection<User>> GetAllUsersAsync(CancellationToken cancellationToken);
        Task<User?> GetUserByIdAsync(int id, CancellationToken cancellationToken);
        Task AddUserAsync(User user, CancellationToken cancellationToken);
        Task UpdateUserAsync(User user, CancellationToken cancellationToken);
        Task<bool> DeleteUserAsync(int id, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(int userId);
    }
}
