using System.Security.Claims;
using System.Text.Encodings.Web;
using BillingAndSubscriptionSystem.Core.Contracts;
using BillingAndSubscriptionSystem.Services.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace BillingAndSubscriptionSystem.WebApi.Authentication
{
    public class CustomAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        IRedisService redisService,
        ITokenService tokenService,
        ILoggerFactory logger,
        UrlEncoder urlEncoder
    ) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, urlEncoder)
    {
        private readonly IRedisService _redisService = redisService;
        private readonly ITokenService _tokenService = tokenService;

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var token = Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
            token ??= Request.Query["token"].FirstOrDefault();

            if (string.IsNullOrEmpty(token))
                return AuthenticateResult.Fail("Unauthorized - No Token Provided");

            var tokenData = await _redisService.GetTokenDataAsync(token, CancellationToken.None);

            if (tokenData == null || string.IsNullOrEmpty(tokenData.Token))
                return AuthenticateResult.Fail("Unauthorized - Token Not Found or Expired");

            bool validToken = _tokenService.VerifyToken(tokenData.Token);
            if (!validToken)
                return AuthenticateResult.Fail("Unauthorized - Invalid Token");

            var principal = CreatePrincipal(tokenData.Token, tokenData.Role);

            Context.User = principal;
            Thread.CurrentPrincipal = principal;
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }

        private ClaimsPrincipal CreatePrincipal(string token, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, token),
                new Claim(ClaimTypes.Authentication, token),
                new Claim(ClaimTypes.NameIdentifier, token),
                new Claim(ClaimTypes.Role, role),
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            return new ClaimsPrincipal(identity);
        }
    }
}
