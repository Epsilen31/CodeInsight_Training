using BillingAndSubscriptionSystem.DataAccess.Contracts;
using BillingAndSubscriptionSystem.Services.DTOs;
using MediatR;

namespace BillingAndSubscriptionSystem.Services.Features.Analytics
{
    public class GetDashboardStatsQuery
    {
        public class Query : IRequest<DashboardStatsDto> { }

        public class Handler : IRequestHandler<Query, DashboardStatsDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<DashboardStatsDto> Handle(
                Query request,
                CancellationToken cancellationToken
            )
            {
                int overduePaymentsCount = await GetOverduePaymentsCount(cancellationToken);
                int inactiveSubscriptionsCount = await GetInactiveSubscriptionsCount(
                    cancellationToken
                );
                int inactiveUsersCount = await GetInactiveUsersCount(cancellationToken);
                int totalPaymentsCount = await GetTotalPaymentsCount(cancellationToken);
                int totalUsersCount = await GetTotalUsersCount(cancellationToken);
                List<MonthlySubscriptionDto> monthlySubscriptionsDto =
                    await GetMonthlySubscriptions(cancellationToken);
                List<PlanTypeCountDto> subscriptionPlanStats = await GetSubscriptionPlanStats(
                    cancellationToken
                );

                return new DashboardStatsDto
                {
                    OverduePayments = overduePaymentsCount,
                    InactiveSubscriptions = inactiveSubscriptionsCount,
                    InactiveUsers = inactiveUsersCount,
                    TotalPayments = totalPaymentsCount,
                    TotalUsers = totalUsersCount,
                    MonthlySubscriptions = monthlySubscriptionsDto,
                    SubscriptionPlanStats = subscriptionPlanStats,
                };
            }

            private async Task<int> GetOverduePaymentsCount(CancellationToken cancellationToken)
            {
                return await _unitOfWork.PaymentRepository.GetOverduePaymentsCountAsync(
                    cancellationToken
                );
            }

            private async Task<int> GetInactiveSubscriptionsCount(
                CancellationToken cancellationToken
            )
            {
                return await _unitOfWork.UserSubscriptionRepository.GetInactiveSubscriptionsCountAsync(
                    cancellationToken
                );
            }

            private async Task<int> GetInactiveUsersCount(CancellationToken cancellationToken)
            {
                return await _unitOfWork.UserRepository.GetInactiveUsersCountAsync(
                    cancellationToken
                );
            }

            private async Task<int> GetTotalPaymentsCount(CancellationToken cancellationToken)
            {
                return await _unitOfWork.PaymentRepository.GetTotalPaymentsCountAsync(
                    cancellationToken
                );
            }

            private async Task<int> GetTotalUsersCount(CancellationToken cancellationToken)
            {
                return await _unitOfWork.UserRepository.GetTotalUsersCountAsync(cancellationToken);
            }

            private async Task<List<MonthlySubscriptionDto>> GetMonthlySubscriptions(
                CancellationToken cancellationToken
            )
            {
                List<MonthlySubscriptionDto> monthlySubscriptionData = (
                    await _unitOfWork.UserSubscriptionRepository.GetMonthlySubscriptionsAsync(
                        cancellationToken
                    )
                )
                    .Select(subscription => new MonthlySubscriptionDto
                    {
                        Month = subscription.Month,
                        Count = subscription.Count,
                    })
                    .ToList();
                return monthlySubscriptionData;
            }

            private async Task<List<PlanTypeCountDto>> GetSubscriptionPlanStats(
                CancellationToken cancellationToken
            )
            {
                List<PlanTypeCountDto> planTypeCountData = (
                    await _unitOfWork.UserSubscriptionRepository.GetSubscriptionPlanCountsAsync(
                        cancellationToken
                    )
                )
                    .Select(planTypeCountData => new PlanTypeCountDto
                    {
                        PlanType = planTypeCountData.PlanType,
                        Count = planTypeCountData.Count,
                    })
                    .ToList();
                return planTypeCountData;
            }
        }
    }
}
