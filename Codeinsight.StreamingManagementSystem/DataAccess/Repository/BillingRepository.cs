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
                @"SELECT Id AS BillingId, BillingAddress, PaymentMethod, UserId FROM BillingDetails WHERE UserId = @UserId";

            var billingDetails = connection.Query<Billing>(query, new { UserId = userId }).AsList();

            if (billingDetails == null)
            {
                throw new InvalidOperationException(
                    $"No billing details found for User ID: {userId}"
                );
            }

            return billingDetails;
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
                    user.Id AS UserId,
                    user.Name AS UserName,
                    user.Email AS UserEmail,
                    user.Phone AS UserPhone,
                    billing.Id AS BillingId,
                    billing.BillingAddress,
                    billing.PaymentMethod
                FROM Users user
                LEFT JOIN BillingDetails billing ON user.Id = billing.UserId";
            return connection.Query<Billing>(query).AsList();
        }
    }
}
