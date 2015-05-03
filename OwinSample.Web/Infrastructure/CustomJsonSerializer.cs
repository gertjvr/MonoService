using Newtonsoft.Json;
using OwinSample.Web.Infrastructure.SignalR;

namespace OwinSample.Web.Infrastructure
{
    public class CustomJsonSerializer : JsonSerializer
    {
        public CustomJsonSerializer()
        {
            ContractResolver = new SignalRContractResolver();
            Formatting = Formatting.Indented;
            DefaultValueHandling = DefaultValueHandling.Ignore;
            DateParseHandling = DateParseHandling.DateTimeOffset;
            DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
        }
    }
}