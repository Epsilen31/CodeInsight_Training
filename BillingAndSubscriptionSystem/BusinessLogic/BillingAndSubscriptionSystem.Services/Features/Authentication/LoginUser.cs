using BillingAndSubscriptionSystem.Core.Contracts;
using BillingAndSubscriptionSystem.Core.Exceptions;
using BillingAndSubscriptionSystem.Core.TokenDatas;
using BillingAndSubscriptionSystem.DataAccess.Contracts;
using BillingAndSubscriptionSystem.Services.Contracts;
using BillingAndSubscriptionSystem.Services.DTOs;
using MapsterMapper;
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
            private readonly IMapper _mapper;

            public Handler(
                IUnitOfWork unitOfWork,
                ILogger<Handler> logger,
                ITokenService tokenService,
                IRedisService redisService,
                IMapper mapper
            )
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
                _tokenService = tokenService;
                _redisService = redisService;
                _mapper = mapper;
            }

            public async Task<LoginDto> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var users = await _unitOfWork.UserRepository.GetAllUsersAsync(
                        cancellationToken
                    );

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

                    request.Login.Role = userDto.Role ?? "User";

                    var token = _tokenService.GenerateToken(request.Login);

                    var tokenData = new TokenDataDto
                    {
                        Token = token,
                        Role = userDto.Role ?? "User",
                    };

                    var mappedTokenData = _mapper.Map<TokenData>(tokenData);

                    await _redisService.SetTokenDataAsync(
                        key: token,
                        tokenData: mappedTokenData,
                        cancellationToken: cancellationToken,
                        expiry: TimeSpan.FromHours(24)
                    );

                    return new LoginDto
                    {
                        Token = token,
                        Email = userDto.Email,
                        Name = userDto.Name,
                        Role = userDto.Role,
                        Id = userDto.Id.ToString(),
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
