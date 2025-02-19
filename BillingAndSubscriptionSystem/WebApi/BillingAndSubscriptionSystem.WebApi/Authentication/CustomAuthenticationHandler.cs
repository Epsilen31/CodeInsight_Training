using System.Security.Claims;
using System.Text.Encodings.Web;
using BillingAndSubscriptionSystem.Core.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace BillingAndSubscriptionSystem.WebApi.Authentication
{
    public class CustomAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        IRedisService redisService,
        ILoggerFactory logger,
        UrlEncoder urlEncoder
    ) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, urlEncoder)
    {
        private readonly IRedisService _redisService = redisService;

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var token = Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                return AuthenticateResult.Fail("Unauthorized - No Token Provided");
            }

            var cachedToken = await _redisService.GetValueAsync(token, CancellationToken.None);
            if (cachedToken == null)
            {
                return AuthenticateResult.Fail("Unauthorized - Token Not Found or Expired");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, cachedToken),
                new Claim(ClaimTypes.Authentication, token),
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
