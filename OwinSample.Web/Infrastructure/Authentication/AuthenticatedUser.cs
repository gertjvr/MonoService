using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Nancy.Security;

namespace OwinSample.Web.Infrastructure.Authentication
{
    public class AuthenticatedUser : IUserIdentity
    {
        private readonly ClaimsIdentity _claimsIdentity;

        public string UserName { get; private set; }

        public IEnumerable<string> Claims
        {
            get { return _claimsIdentity.Claims.Select(c => c.ToString()); }
        }

        public Guid PersonId { get; private set; }

        public AuthenticatedUser(ClaimsIdentity claimsIdentity)
        {
            _claimsIdentity = claimsIdentity;
            UserName = _claimsIdentity.Claims.Single(p => p.Type == ClaimTypes.Name).Value;
            PersonId = Guid.Parse(_claimsIdentity.Claims.Single(p => p.Type == ClaimTypes.NameIdentifier).Value);
        }
    }
}