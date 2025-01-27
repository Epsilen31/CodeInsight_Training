namespace Codeinsight.StreamingManagementSystem.BusinessLogic.Enums
{
    public enum BillingAndSubscriptionOperations
    {
        None,
        ManageSubscription = 1,
        ManageBilling = 2,
        ManagePayment = 3,
    }

    public enum SubscriptionOperations
    {
        None,
        AddSubscription = 1,
        UpdateSubscription = 2,
    }

    public enum PaymentOperations
    {
        None,
        UpdatePayment = 1,
        GetOverduePayment = 2,
    }
}
