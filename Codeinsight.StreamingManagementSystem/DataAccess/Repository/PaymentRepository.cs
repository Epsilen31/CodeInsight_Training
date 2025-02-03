using Codeinsight.StreamingManagementSystem.DataAccess.Context;
using Codeinsight.StreamingManagementSystem.DataAccess.Contracts;
using Codeinsight.StreamingManagementSystem.DataAccess.Entities;
using Dapper;

namespace Codeinsight.StreamingManagementSystem.DataAccess.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly DatabaseConnection _context;

        public PaymentRepository(DatabaseConnection context)
        {
            _context = context;
        }

        public void ProcessPayment(Payment payment)
        {
            using var connection = _context.Connection;

            string query =
                @"
                INSERT INTO Payments (SubscriptionId, Amount, PaymentDate, Status)
                VALUES (@SubscriptionId, @Amount, @PaymentDate, @Status)";

            connection.Execute(
                query,
                new
                {
                    payment.SubscriptionId,
                    payment.Amount,
                    payment.PaymentDate,
                    Status = payment.Status.ToString(),
                }
            );
        }

        public ICollection<Payment> GetOverduePayments()
        {
            using var connection = _context.Connection;

            string query =
                @"
                SELECT Payments.*
                FROM Payments
                INNER JOIN Subscriptions ON Payments.SubscriptionId = Subscriptions.Id
                WHERE Payments.Status = 'Overdue'";

            return connection.Query<Payment>(query).AsList();
        }
    }
}
