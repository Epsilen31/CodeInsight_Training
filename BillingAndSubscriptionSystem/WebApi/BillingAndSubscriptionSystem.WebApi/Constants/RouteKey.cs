namespace BillingAndSubscriptionSystem.WebApi.Constants
{
    public class RouteKey
    {
        public const string MainRoute = "api/BillingAndSubscription/[controller]";

        // UserSubscriptionRoute
        public const string GetSubscriptionByUserId = "GetSubscriptionByUserId/{id}";
        public const string CreateUserSubscriptionPlan = "CreateUserSubscriptionPlan";
        public const string UpdateUserSubscriptionPlan = "UpdateUserSubscriptionPlan";

        // PaymentRoute
        public const string CreatePayment = "CreatePayment";
        public const string GetOverDuePayment = "GetOverDuePayment";

        // BillingRoute
        public const string GetUsersWithBilling = "GetUsersWithBilling";
        public const string UpdateBilling = "UpdateBilling";
    }
}
