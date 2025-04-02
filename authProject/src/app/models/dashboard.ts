export interface IDashboardResponseData {
  overduePayments: number;
  inactiveSubscriptions: number;
  inactiveUsers: number;
  totalPayments: number;
  totalUsers: number;
  monthlySubscriptions: Array<{ month: string; count: number }>;
  subscriptionPlanStats: Array<{ planType: string; count: number }>;
}
