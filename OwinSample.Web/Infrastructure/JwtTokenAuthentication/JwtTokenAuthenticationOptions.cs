using Microsoft.Owin.Security;

namespace OwinSample.Web.Infrastructure.JwtTokenAuthentication
{
    public class JwtTokenAuthenticationOptions : AuthenticationOptions
    {
        public JwtTokenAuthenticationOptions(
            string issuer = null, 
            string audience = null, 
            string clientSecret = null, 
            AuthenticationMode authenticationMode = AuthenticationMode.Active, 
            string authenticationType = "Bearer")
            : base(authenticationType)
        {
            Issuer = issuer;
            Audience = audience;
            SymmetricKey = clientSecret;
            AuthenticationMode = authenticationMode;
            AuthenticationType = authenticationType;
        }

        /// <summary>
        /// The Issuer of the JWT token - grab a token (https://docs.auth0.com/protocols) and use https://jwt.io to see the issuer
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// The Client Id from your Application
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// The Client Secret from your Application
        /// </summary>
        public string SymmetricKey { get; set; }
    }
}