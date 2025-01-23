using Codeinsight.StreamingManagementSystem.DataAccess.Context;
using Codeinsight.StreamingManagementSystem.DataAccess.Contracts;

namespace Codeinsight.StreamingManagementSystem.DataAccess.Repository
{
    public class UserSubscriptionRepository : IUserSubscriptionRepository
    {
        private readonly DatabaseConnection _context;

        public UserSubscriptionRepository(DatabaseConnection context)
        {
            _context = context;
        }
        // CRUD operation logic in dapper
    }
}
