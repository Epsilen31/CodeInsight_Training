using BillingAndSubscriptionSystem.Core.Exceptions;
using BillingAndSubscriptionSystem.DataAccess.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BillingAndSubscriptionSystem.Services.Features.Users
{
    public class GetAllUsers
    {
        public class Query
            : IRequest<ICollection<BillingAndSubscriptionSystem.Entities.Entities.User>>
        {
            public Query() { }
        }

        public class Handler
            : IRequestHandler<
                Query,
                ICollection<BillingAndSubscriptionSystem.Entities.Entities.User>
            >
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ILogger<Handler> _logger;

            public Handler(IUnitOfWork unitOfWork, ILogger<Handler> logger)
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
            }

            public async Task<
                ICollection<BillingAndSubscriptionSystem.Entities.Entities.User>
            > Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    var users = await _unitOfWork.UserRepository.GetAllUsersAsync(
                        cancellationToken
                    );

                    if (users == null || users.Count == 0)
                    {
                        _logger.LogWarning("No users found in the database.");
                        return new List<BillingAndSubscriptionSystem.Entities.Entities.User>();
                    }
                    return users.ToList();
                }
                catch (Exception exception)
                {
                    _logger.LogError(
                        exception,
                        "Error fetching users: {Message}",
                        exception.Message
                    );
                    throw new CustomException("Error fetching users.", exception);
                }
            }
        }
    }
}
