using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;

namespace Demo.Extension
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IConfiguration _configuration;
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, IConfiguration configuration) : base(options, logger, encoder)
        {
            _configuration = configuration;
        }
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue("Authorization", out var authHeader))
                return Task.FromResult(AuthenticateResult.Fail("Authorization header missing"));


            if (authHeader.Count == 0 || !authHeader[0].StartsWith("Basic "))
                return Task.FromResult(AuthenticateResult.Fail("Invalid authorization scheme"));


            var token = authHeader[0]["Basic ".Length..].Trim();
            var credentialBytes = Convert.FromBase64String(token);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);

            if (credentials.Length != 2)
                return Task.FromResult(AuthenticateResult.Fail("Invalid credentials format"));

            var username = credentials[0];
            var password = credentials[1];

            string actualusername = _configuration["ApiCredentials:UserName"];
            string actualpassword = _configuration["ApiCredentials:Password"];

            if (username != actualusername || password != actualpassword)
                return Task.FromResult(AuthenticateResult.Fail("Invalid credentials."));

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, username),
            };

            var claimsIdentity = new ClaimsIdentity(claims, Scheme.Name);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
        }
    }
}
