using Nimbus.MessageContracts;

namespace OwinSample.MessageContracts.Requests
{
    public class AuthenticateUserRequest : IBusRequest<AuthenticateUserRequest, AuthenticateUserRespone>
    {
        public AuthenticateUserRequest(string emailAddress, string password)
        {
            EmailAddress = emailAddress;
            Password = password;
        }

        public string EmailAddress { get; private set; }

        public string Password { get; private set; }
    }
}