namespace BillingAndSubscriptionSystem.Services.DTOs
{
    public class DashboardStatsDto
    {
        public int OverduePayments { get; set; }
        public int InactiveSubscriptions { get; set; }
        public int InactiveUsers { get; set; }

        public int TotalPayments { get; set; }
        public int TotalUsers { get; set; }
        public List<MonthlySubscriptionDto> MonthlySubscriptions { get; set; } = [];

        public List<PlanTypeCountDto> SubscriptionPlanStats { get; set; } = [];
    }
}
