using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Nimbus;
using OwinSample.MessageContracts.Requests;
using OwinSample.Web.ConfigurationSettings;
using OwinSample.Web.Infrastructure.Clock;
using OwinSample.Web.Infrastructure.JwtTokenAuthentication;

namespace OwinSample.Web.Infrastructure.Authentication
{
    public class Authenticator
    {
        private readonly IBus _bus;
        private readonly IClock _clock;
        private readonly IssuerSetting _issuer;
        private readonly AudienceSetting _audience;
        private readonly ClientSecretSetting _clientSecret;

        public Authenticator(
            IBus bus,
            IClock clock,
            IssuerSetting issuer,
            AudienceSetting audience,
            ClientSecretSetting clientSecret)
        {
            _bus = bus;
            _clock = clock;
            _issuer = issuer;
            _audience = audience;
            _clientSecret = clientSecret;
        }

        public async Task<string> Authenticate(string emailAddress, string password)
        {
            var response = await _bus.Request(new AuthenticateUserRequest (emailAddress, password));
            
            if (response == null) return null;

            var token = new AuthToken
            {
                Issuer = _issuer,
                Audience = _audience,
                Claims = new Dictionary<string, object>
                {
                    {ClaimTypes.NameIdentifier, response.Id},
                    {ClaimTypes.Name, response.Name},
                    {ClaimTypes.Email, response.EmailAddress}
                },
                Expiry = _clock.UtcNow.AddDays(7),
            };

            return JsonWebToken.Encode(token, _clientSecret, JwtHashAlgorithm.HS256);
        }
    }
}