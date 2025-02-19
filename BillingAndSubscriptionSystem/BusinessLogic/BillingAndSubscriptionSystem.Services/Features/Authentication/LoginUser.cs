using BillingAndSubscriptionSystem.Core.Contracts;
using BillingAndSubscriptionSystem.Core.Exceptions;
using BillingAndSubscriptionSystem.DataAccess;
using BillingAndSubscriptionSystem.Services.Contracts;
using BillingAndSubscriptionSystem.Services.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BillingAndSubscriptionSystem.Services.Features.Authentication
{
    public class LoginUser
    {
        public class Command : IRequest<LoginDto>
        {
            public LoginDto Login { get; set; }

            public Command(LoginDto loginDto)
            {
                Login = loginDto;
            }
        }

        public class Handler : IRequestHandler<Command, LoginDto>
        {
            private readonly UnitOfWork _unitOfWork;
            private readonly ILogger<LoginUser> _logger;
            private readonly IRedisService _redisService;
            private readonly ITokenService _tokenService;

            public Handler(
                UnitOfWork unitOfWork,
                ILogger<LoginUser> logger,
                ITokenService tokenService,
                IRedisService redisService
            )
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
                _tokenService = tokenService;
                _redisService = redisService;
            }

            public async Task<LoginDto> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var users = await _unitOfWork.UserRepository.GetAllUsersAsync(
                        cancellationToken
                    );
                    var user = users.FirstOrDefault(user => user.Email == request.Login.Email);

                    if (user == null)
                        throw new CustomException("User not found.", null);

                    if (!BCrypt.Net.BCrypt.Verify(request.Login.Password, user.Password))
                        throw new CustomException("Invalid password.", null);

                    if (user.Email == null)
                        throw new CustomException("User email is Empty.", null);

                    var token = _tokenService.GenerateToken(request.Login);

                    var serializedUser = Newtonsoft.Json.JsonConvert.SerializeObject(user);

                    var tokenExpiry = TimeSpan.FromHours(1);

                    await _redisService.SetValueAsync(token, serializedUser, cancellationToken);

                    return new LoginDto { Token = token };
                }
                catch (Exception exception)
                {
                    _logger.LogError(
                        exception,
                        "Error logging in user: {Message}",
                        exception.Message
                    );
                    throw new CustomException("Error logging in user.", exception);
                }
            }
        }
    }
}
