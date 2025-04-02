namespace BillingAndSubscriptionSystem.WebApi.Constants
{
    public class RouteKey
    {
        // Base Route
        public const string MainRoute = "api/BillingAndSubscription";

        // Billing Routes
        public const string BillingRoute = MainRoute + "/Billing";
        public const string GetUsersWithBilling = "GetUsersWithBilling";
        public const string UpdateBilling = "UpdateBilling";

        // Payment Routes
        public const string PaymentRoute = MainRoute + "/Payments";
        public const string CreatePayment = "CreatePayment";
        public const string GetOverDuePayment = "GetOverduePayments";

        // Subscription Routes
        public const string UserSubscriptionRoute = MainRoute + "/UserSubscription";
        public const string GetSubscriptionByUserId = "GetSubscriptionByUserId/{userId}";
        public const string CreateUserSubscriptionPlan = "CreateUserSubscriptionPlan";
        public const string UpdateUserSubscriptionPlan = "UpdateUserSubscriptionPlan/{Id}";
        public const string DeleteUserSubscriptionPlan = "DeleteUserSubscriptionPlan/{Id}";

        // User Routes
        public const string UserRoute = MainRoute + "/User";
        public const string GetUsers = "GetUsers";
        public const string GetUserById = "GetUserById/{userId}";
        public const string AddUser = "AddUser";
        public const string UpdateUser = "UpdateUser/{userId}";
        public const string DeleteUser = "DeleteUser/{userId}";

        // Auth Routes
        public const string AuthRoute = MainRoute + "/Auth";
        public const string Login = "Login";
        public const string Register = "Register";

        // Menu Routes
        public const string MenuRoute = MainRoute + "/Menu";
        public const string GetSidebarMenu = "GetSidebarMenu";

        // SubMenu Routes
        public const string SubMenuRoute = MainRoute + "/SubMenu";
        public const string GetAllSubMenu = "GetAllSubMenu";

        // Analytics Routes
        public const string AnalyticsRoute = MainRoute + "/Analytics";
        public const string GetAnalyticsState = "AnalyticsState";
    }
}
