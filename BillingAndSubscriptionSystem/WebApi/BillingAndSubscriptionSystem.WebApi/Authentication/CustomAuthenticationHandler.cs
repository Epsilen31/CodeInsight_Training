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
            // Check if token is present in the request header
            var token = Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

            // if there is no token in the header then it will cheack in the query paremeter
            token ??= Request.Query["token"].FirstOrDefault();

            if (string.IsNullOrEmpty(token))
                return AuthenticateResult.Fail("Unauthorized - No Token Provided");

            var cachedToken = await _redisService.GetValueAsync(token, CancellationToken.None);

            if (cachedToken == null)
                return AuthenticateResult.Fail("Unauthorized - Token Not Found or Expired");

            bool validToken = _tokenService.VerifyToken(token);
            if (!validToken)
                return AuthenticateResult.Fail("Unauthorized - Invalid Token");

            var principal = CreatePrincipal(cachedToken);

            Context.User = principal;
            Thread.CurrentPrincipal = principal;
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }

        private ClaimsPrincipal CreatePrincipal(string token)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, token),
                new Claim(ClaimTypes.Authentication, token),
                new Claim(ClaimTypes.NameIdentifier, token),
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            return principal;
        }
    }
}
