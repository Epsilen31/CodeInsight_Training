using Codeinsight.StreamingManagementSystem.DataAccess.Context;
using Codeinsight.StreamingManagementSystem.DataAccess.Contracts;

namespace Codeinsight.StreamingManagementSystem.DataAccess.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseConnection _context;

        public UserRepository(DatabaseConnection context)
        {
            _context = context;
        }
        // CRUD operation logic in dapper
    }
}
