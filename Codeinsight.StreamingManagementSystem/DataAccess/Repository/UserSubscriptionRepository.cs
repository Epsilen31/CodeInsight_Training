using Codeinsight.StreamingManagementSystem.DataAccess.Context;
using Codeinsight.StreamingManagementSystem.DataAccess.Contracts;
using Codeinsight.StreamingManagementSystem.DataAccess.Entities;
using Dapper;

namespace Codeinsight.StreamingManagementSystem.DataAccess.Repository
{
    public class UserSubscriptionRepository : IUserSubscriptionRepository
    {
        private readonly DatabaseConnection _context;

        public UserSubscriptionRepository(DatabaseConnection context)
        {
            _context = context;
        }

        public void CreateSubscription(Subscription subscription)
        {
            using var connection = _context.Connection;

            string query =
                "INSERT INTO Subscriptions (UserId, PlanType, StartDate, EndDate, Status) VALUES (@UserId, @PlanType, @StartDate, @EndDate, @SubscriptionStatus)";
            var parameters = new
            {
                subscription.UserId,
                PlanType = subscription.PlanType.ToString(),
                subscription.StartDate,
                subscription.EndDate,
                SubscriptionStatus = subscription.SubscriptionStatus.ToString(),
            };
            connection.Execute(query, parameters);
        }

        public void UpdateSubscription(Subscription subscription)
        {
            using var connection = _context.Connection;

            string query =
                "UPDATE Subscriptions SET UserId = @UserId, PlanType = @PlanType, StartDate = @StartDate, EndDate = @EndDate, Status = @SubscriptionStatus WHERE Id = @Id";

            var parameters = new
            {
                subscription.Id,
                subscription.UserId,
                PlanType = subscription.PlanType.ToString(),
                subscription.StartDate,
                subscription.EndDate,
                SubscriptionStatus = subscription.SubscriptionStatus.ToString(),
            };

            connection.Execute(query, parameters);
        }

        public ICollection<Subscription> GetSubscriptionsByUserId(int userId)
        {
            using var connection = _context.Connection;

            string query =
                @"
                SELECT subscription.*
                FROM Subscriptions subscription
                INNER JOIN Users user ON subscription.UserId = user.Id
                WHERE subscription.UserId = @UserId";

            return connection.Query<Subscription>(query, new { UserId = userId }).AsList();
        }
    }
}
