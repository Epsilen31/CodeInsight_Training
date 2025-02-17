using BillingAndSubscriptionSystem.Core.Exceptions;
using BillingAndSubscriptionSystem.DataAccess;
using BillingAndSubscriptionSystem.Services.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BillingAndSubscriptionSystem.Services.Features.Users
{
    public class AddUser
    {
        public class Query : IRequest<Unit>
        {
            public UserDto User { get; }

            public Query(UserDto user)
            {
                User = user;
            }
        }

        public class Handler : IRequestHandler<Query, Unit>
        {
            private readonly UnitOfWork _unitOfWork;
            private readonly ILogger<AddUser> _logger;

            public Handler(UnitOfWork unitOfWork, ILogger<AddUser> logger)
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
            }

            public async Task<Unit> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    var userEntity = new Entities.Entities.User
                    {
                        Id = request.User.Id,
                        Name = request.User.Name,
                        Email = request.User.Email,
                        Phone = request.User.Phone,
                    };
                    await _unitOfWork.UserRepository.AddUserAsync(userEntity, cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    return Unit.Value;
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Error adding user : {Message}", exception.Message);
                    throw new CustomException("Error adding user.", exception);
                }
            }
        }
    }
}
