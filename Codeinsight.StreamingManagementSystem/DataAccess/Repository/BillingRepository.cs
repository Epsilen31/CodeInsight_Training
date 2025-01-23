using Codeinsight.StreamingManagementSystem.DataAccess.Context;
using Codeinsight.StreamingManagementSystem.DataAccess.Contracts;

namespace Codeinsight.StreamingManagementSystem.DataAccess.Repository
{
    public class BillingRepository : IBillingRepository
    {
        private readonly DatabaseConnection _context;

        public BillingRepository(DatabaseConnection context)
        {
            _context = context;
        }
        // CRUD operation logic in dapper
    }
}
