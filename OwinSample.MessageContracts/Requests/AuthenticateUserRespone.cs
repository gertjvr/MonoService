using System;
using Nimbus.MessageContracts;

namespace OwinSample.MessageContracts.Requests
{
    public class AuthenticateUserRespone : IBusResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string EmailAddress { get; set; }
    }
}