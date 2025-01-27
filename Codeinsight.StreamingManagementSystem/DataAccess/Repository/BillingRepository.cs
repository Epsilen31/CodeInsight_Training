using Codeinsight.StreamingManagementSystem.DataAccess.Context;
using Codeinsight.StreamingManagementSystem.DataAccess.Contracts;
using Codeinsight.StreamingManagementSystem.DataAccess.Entities;
using Dapper;

namespace Codeinsight.StreamingManagementSystem.DataAccess.Repository
{
    public class BillingRepository : IBillingRepository
    {
        private readonly DatabaseConnection _context;

        public BillingRepository(DatabaseConnection context)
        {
            _context = context;
        }

        public ICollection<Billing> GetBillingWithUserDetails(int userId)
        {
            using var connection = _context.Connection;
            string query =
                @"
                SELECT
                    u.Id AS UserId,
                    u.Name AS UserName,
                    u.Email AS UserEmail,
                    b.Id AS BillingId,
                    b.BillingAddress,
                    b.PaymentMethod
                FROM Users u
                INNER JOIN BillingDetails b ON u.Id = b.UserId
                WHERE u.Id = @UserId";

            return connection.Query<Billing>(query, new { userId }).AsList();
        }

        public void UpdateBillingDetails(Billing billingDetails)
        {
            using var connection = _context.Connection;
            string query =
                "UPDATE BillingDetails SET BillingAddress = @BillingAddress, PaymentMethod = @PaymentMethod WHERE UserId = @UserId";
            var parameters = new
            {
                billingDetails.UserId,
                billingDetails.BillingAddress,
                PaymentMethod = billingDetails.PaymentMethod.ToString(),
            };

            Console.WriteLine(
                $"UserId: {billingDetails.UserId}, BillingAddress: {billingDetails.BillingAddress}, PaymentMethod: {billingDetails.PaymentMethod.ToString()}"
            );

            connection.Execute(query, parameters);
        }

        public ICollection<Billing> GetAllUsersWithBillingDetails()
        {
            using var connection = _context.Connection;
            string query =
                @"
                SELECT
                    u.Id AS UserId,
                    u.Name AS UserName,
                    u.Email AS UserEmail,
                    u.Phone AS UserPhone,
                    b.Id AS BillingId,
                    b.BillingAddress,
                    b.PaymentMethod
                FROM Users u
                LEFT JOIN BillingDetails b ON u.Id = b.UserId";

            return connection.Query<Billing>(query).AsList();
        }
    }
}
