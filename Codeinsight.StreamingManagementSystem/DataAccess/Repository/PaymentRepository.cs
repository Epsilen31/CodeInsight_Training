using Codeinsight.StreamingManagementSystem.DataAccess.Context;
using Codeinsight.StreamingManagementSystem.DataAccess.Contracts;

namespace Codeinsight.StreamingManagementSystem.DataAccess.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly DatabaseConnection _context;

        public PaymentRepository(DatabaseConnection context)
        {
            _context = context;
        }
        // CRUD operation logic in dapper
    }
}
