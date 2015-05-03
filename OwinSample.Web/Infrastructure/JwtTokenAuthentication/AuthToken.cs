using System;
using System.Collections.Generic;

namespace OwinSample.Web.Infrastructure.JwtTokenAuthentication
{
    public class AuthToken
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public DateTimeOffset Expiry { get; set; }
        public Dictionary<string, object> Claims { get; set; }
    }
}