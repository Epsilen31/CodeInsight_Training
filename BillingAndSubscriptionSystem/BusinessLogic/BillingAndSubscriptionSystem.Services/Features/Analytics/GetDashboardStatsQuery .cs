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
                int overduePaymentsCount =
                    await _unitOfWork.PaymentRepository.GetOverduePaymentsCountAsync(
                        cancellationToken
                    );
                int inactiveSubscriptionsCount =
                    await _unitOfWork.UserSubscriptionRepository.GetInactiveSubscriptionsCountAsync(
                        cancellationToken
                    );
                int inactiveUsersCount =
                    await _unitOfWork.UserRepository.GetInactiveUsersCountAsync(cancellationToken);
                int totalPaymentsCount =
                    await _unitOfWork.PaymentRepository.GetTotalPaymentsCountAsync(
                        cancellationToken
                    );
                int totalUsersCount = await _unitOfWork.UserRepository.GetTotalUsersCountAsync(
                    cancellationToken
                );

                var monthlySubscriptionData =
                    await _unitOfWork.UserSubscriptionRepository.GetMonthlySubscriptionsAsync(
                        cancellationToken
                    );
                var monthlySubscriptionsDto = monthlySubscriptionData
                    .Select(subscription => new MonthlySubscriptionDto
                    {
                        Month = subscription.Month,
                        Count = subscription.Count,
                    })
                    .ToList();

                var planTypeCountData =
                    await _unitOfWork.UserSubscriptionRepository.GetSubscriptionPlanCountsAsync(
                        cancellationToken
                    );
                var subscriptionPlanStats = planTypeCountData
                    .Select(subscription => new PlanTypeCountDto
                    {
                        PlanType = subscription.PlanType,
                        Count = subscription.Count,
                    })
                    .ToList();

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
        }
    }
}
