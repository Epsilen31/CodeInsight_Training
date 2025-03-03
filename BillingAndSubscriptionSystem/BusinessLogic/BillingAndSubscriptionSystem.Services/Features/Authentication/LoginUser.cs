using BillingAndSubscriptionSystem.Core.Contracts;
using BillingAndSubscriptionSystem.Core.Exceptions;
using BillingAndSubscriptionSystem.DataAccess.Contracts;
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
            private readonly IUnitOfWork _unitOfWork;
            private readonly ILogger<Handler> _logger;
            private readonly IRedisService _redisService;
            private readonly ITokenService _tokenService;

            public Handler(
                IUnitOfWork unitOfWork,
                ILogger<Handler> logger,
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
                    // ✅ Fetch all users (now includes roles)
                    var users = await _unitOfWork.UserRepository.GetAllUsersAsync(
                        cancellationToken
                    );

                    // ✅ Find the user and map to `UserDto`
                    var userDto = users
                        .Where(user => user.Email == request.Login.Email)
                        .Select(user => new UserDto
                        {
                            Id = user.Id,
                            Name = user.Name,
                            Email = user.Email,
                            Phone = user.Phone,
                            Password = user.Password,
                            Role = user.Role != null ? user.Role.RoleName : "User",
                        })
                        .FirstOrDefault();

                    if (userDto == null)
                        throw new CustomException("User not found.", null);

                    if (!BCrypt.Net.BCrypt.Verify(request.Login.Password, userDto.Password))
                        throw new CustomException("Invalid password.", null);

                    var token = _tokenService.GenerateToken(request.Login);

                    var serializedUser = Newtonsoft.Json.JsonConvert.SerializeObject(
                        new
                        {
                            userDto.Id,
                            userDto.Email,
                            userDto.Role,
                        }
                    );

                    await _redisService.SetValueAsync(token, serializedUser, cancellationToken);

                    return new LoginDto
                    {
                        Token = token,
                        Email = userDto.Email,
                        Name = userDto.Name,
                        Role = userDto.Role,
                    };
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
