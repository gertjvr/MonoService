using Microsoft.Owin;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security.Infrastructure;
using Owin;

namespace OwinSample.Web.Infrastructure.JwtTokenAuthentication
{
    public class JwtTokenAuthenticationMiddleware : AuthenticationMiddleware<JwtTokenAuthenticationOptions>
    {
        private readonly ILogger _logger;

        public JwtTokenAuthenticationMiddleware(
            OwinMiddleware next,
            IAppBuilder app,
            JwtTokenAuthenticationOptions options)
            : base(next, options)
        {
            _logger = app.CreateLogger<JwtTokenAuthenticationMiddleware>();
        }

        protected override AuthenticationHandler<JwtTokenAuthenticationOptions> CreateHandler()
        {
            return new JwtTokenAuthenticationHandler(_logger);
        }
    }
}