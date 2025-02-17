using BillingAndSubscriptionSystem.Core.Exceptions;
using BillingAndSubscriptionSystem.DataAccess;
using BillingAndSubscriptionSystem.Entities.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BillingAndSubscriptionSystem.Services.Features.Users
{
    public class GetAllUsers
    {
        public class Query : IRequest<ICollection<User>>
        {
            public Query() { }
        }

        public class Handler : IRequestHandler<Query, ICollection<User>>
        {
            private readonly UnitOfWork _unitOfWork;
            private readonly ILogger<GetAllUsers> _logger;

            public Handler(UnitOfWork unitOfWork, ILogger<GetAllUsers> logger)
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
            }

            public async Task<ICollection<User>> Handle(
                Query request,
                CancellationToken cancellationToken
            )
            {
                try
                {
                    var users = await _unitOfWork.UserRepository.GetAllUsersAsync();

                    if (users == null || users.Count == 0)
                    {
                        _logger.LogWarning("No users found in the database.");
                        return [];
                    }
                    return [.. users];
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
